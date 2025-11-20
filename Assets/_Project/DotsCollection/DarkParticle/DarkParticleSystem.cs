using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct ParticleLifeTimeUpdateJob : IJobEntity
    {
        public float deltaTime;
        [NativeDisableParallelForRestriction] public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, Entity e, ref Particle particleLifeTime)
        {
            if (particleLifeTime.isInfiniteLoop)
            {
                particleLifeTime.timeCount -= particleLifeTime.timeCount > particleLifeTime.lifeTime ? particleLifeTime.lifeTime : 0;
            }
            else
            {
                if (particleLifeTime.timeCount > particleLifeTime.lifeTime)
                {
                    particleLifeTime.loopCount--;
                    particleLifeTime.timeCount -= particleLifeTime.lifeTime;
                    if (particleLifeTime.loopCount <= 0)
                        ecb.SetComponentEnabled<Particle>(chunk, e, false);
                    return;
                }
            }
            particleLifeTime.timeCount += deltaTime;
            particleLifeTime.currentRate = math.clamp(particleLifeTime.timeCount / particleLifeTime.lifeTime, 0, 1);
        }
    }

    [BurstCompile]
    public partial struct ParticleSetupStartRotationJob : IJobEntity
    {
        [NativeDisableParallelForRestriction] public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, Entity entity, ref ParticleRotationOverTime rotationOverTime, in ParticleStartRotation startRotation, in LocalTransform localTransform)
        {
            rotationOverTime.startRotation = localTransform.Rotation;
            ecb.SetComponentEnabled<ParticleStartRotation>(chunk, entity, false);
        }
    }

    [BurstCompile]
    public partial struct ParticleSetupStartSizeJob : IJobEntity
    {
        [NativeDisableParallelForRestriction] public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, Entity entity, in ParticleStartSize particleStartSize, ref ParticleSizeOverTime sizeOverTime, in LocalTransform localTransform)
        {
            sizeOverTime.maxSize = localTransform.Scale;
            ecb.SetComponentEnabled<ParticleStartSize>(chunk, entity, false);
        }
    }

    [BurstCompile]
    public partial struct ParticleSetupMovementJob : IJobEntity
    {
        [NativeDisableParallelForRestriction] public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, Entity entity, ref ParticleStartPosition particleStartPos, ref ParticleMovementOvertime movementOverTime, in LocalTransform localTransform)
        {
            if (particleStartPos.updatedStartPos == false)
            {
                movementOverTime.startPosition = localTransform.Position;
                particleStartPos.updatedStartPos = true;
            }
            movementOverTime.movementDirection = math.mul(localTransform.Rotation, movementOverTime.relativeDirection);
            if (particleStartPos.shouldKeepEnabled == false)
                ecb.SetComponentEnabled<ParticleStartSize>(chunk, entity, false);
        }
    }

    [BurstCompile]
    public partial struct ParticleSizeOverTimeJob : IJobEntity
    {
        public void Execute(in Particle particle, in ParticleSizeOverTime sizeOverTime, ref LocalTransform localTransform)
        {
            ref var curve = ref sizeOverTime.data.Value;
            localTransform.Scale = curve.Evaluate(particle.currentRate) * sizeOverTime.maxSize;
        }
    }

    [BurstCompile]
    public partial struct ParticleRotationOverTimeJob : IJobEntity
    {
        public void Execute(in Particle particle, in ParticleRotationOverTime rotationOverTime, ref LocalTransform localTransform)
        {
            ref var curve = ref rotationOverTime.data.Value;
            localTransform.Rotation = math.mul(rotationOverTime.startRotation, quaternion.AxisAngle(rotationOverTime.axis, curve.Evaluate(particle.currentRate) * rotationOverTime.maxAngles));
        }
    }

    [BurstCompile]
    public partial struct ParticleMovementOverTimeJob : IJobEntity
    {
        public void Execute(in Particle particle, ref ParticleMovementOvertime movementOverTime, ref LocalTransform localTransform)
        {
            ref var curve = ref movementOverTime.data.Value;

            localTransform.Position = movementOverTime.startPosition + curve.Evaluate(particle.currentRate) * movementOverTime.movementDirection * movementOverTime.maxDistance;
        }
    }

    [BurstCompile]
    public partial struct DarkParticleSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
        }
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
            var ecbParallel = ecb.AsParallelWriter();
            var lifetimeJob = new ParticleLifeTimeUpdateJob
            {
                deltaTime = deltaTime,
                ecb = ecbParallel,
            };

            var handle = lifetimeJob.ScheduleParallel(state.Dependency);
            //scale
            var setupScaleJob = new ParticleSetupStartSizeJob
            {
                ecb = ecbParallel
            };
            handle = setupScaleJob.ScheduleParallel(handle);

            //rotation
            var setupRotationJob = new ParticleSetupStartRotationJob
            {
                ecb = ecbParallel
            };

            handle = setupRotationJob.ScheduleParallel(handle);
            //position
            var setupStartPosJob = new ParticleSetupMovementJob
            {
                ecb = ecbParallel
            };
            handle = setupStartPosJob.ScheduleParallel(handle);

            handle.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();

            var sizeJob = new ParticleSizeOverTimeJob();
            sizeJob.ScheduleParallel();

            var rotationJob = new ParticleRotationOverTimeJob();
            rotationJob.ScheduleParallel();

            var movementJob = new ParticleMovementOverTimeJob();
            movementJob.ScheduleParallel();
        }
    }
}
