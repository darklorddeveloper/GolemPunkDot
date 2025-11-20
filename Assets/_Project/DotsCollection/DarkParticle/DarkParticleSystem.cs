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
        public void Execute(EnabledRefRW<Particle> particleEnable, ref Particle particleLifeTime)
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
                        particleEnable.ValueRW = false;
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
        public void Execute(EnabledRefRW<ParticleStartRotation> startRotation, ref ParticleRotationOverTime rotationOverTime, in LocalTransform localTransform)
        {
            rotationOverTime.startRotation = localTransform.Rotation;
            startRotation.ValueRW = false;
        }
    }

    [BurstCompile]
    public partial struct ParticleSetupStartSizeJob : IJobEntity
    {
        public void Execute(EnabledRefRW<ParticleStartSize> particleStartSize, ref ParticleSizeOverTime sizeOverTime, in LocalTransform localTransform)
        {
            sizeOverTime.maxSize = localTransform.Scale;
            particleStartSize.ValueRW = false;
        }
    }

    [BurstCompile]
    public partial struct ParticleSetupMovementJob : IJobEntity
    {
        public void Execute(EnabledRefRW<ParticleStartPosition> startSize, ref ParticleStartPosition particleStartPos, ref ParticleMovementOvertime movementOverTime, in LocalTransform localTransform)
        {
            if (particleStartPos.updatedStartPos == false)
            {
                movementOverTime.startPosition = localTransform.Position;
                particleStartPos.updatedStartPos = true;
            }
            movementOverTime.movementDirection = math.mul(localTransform.Rotation, movementOverTime.relativeDirection);
            if (particleStartPos.shouldKeepEnabled == false)
                startSize.ValueRW = false;
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
    public partial struct ParticleColorOverTimeJob : IJobEntity
    {
        public void Execute(in Particle particle, ref ParticleColorOverTime particleColorOverTime)
        {
            ref var curve = ref particleColorOverTime.data.Value;
            particleColorOverTime.Value = curve.Evaluate(particle.currentRate);
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

            var lifetimeJob = new ParticleLifeTimeUpdateJob
            {
                deltaTime = deltaTime,
            };

            lifetimeJob.ScheduleParallel();
            //scale
            var setupScaleJob = new ParticleSetupStartSizeJob();
            setupScaleJob.ScheduleParallel();
            //rotation
            var setupRotationJob = new ParticleSetupStartRotationJob();
            setupRotationJob.ScheduleParallel();
            //position
            var setupStartPosJob = new ParticleSetupMovementJob();
            setupStartPosJob.ScheduleParallel();

            var sizeJob = new ParticleSizeOverTimeJob();
            sizeJob.ScheduleParallel();

            var rotationJob = new ParticleRotationOverTimeJob();
            rotationJob.ScheduleParallel();

            var movementJob = new ParticleMovementOverTimeJob();
            movementJob.ScheduleParallel();

            var colorOvertimeJob = new ParticleColorOverTimeJob();
            colorOvertimeJob.ScheduleParallel();
        }
    }
}
