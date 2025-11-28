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
            particleLifeTime.timeCount += deltaTime;
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
                    {
                        particleEnable.ValueRW = false;
                        return;
                    }
                }
            }
            particleLifeTime.currentRate = math.clamp(particleLifeTime.timeCount / particleLifeTime.lifeTime, 0, 1);
        }
    }

    [BurstCompile]
    public partial struct ParticleSetupStartRotationJob : IJobEntity
    {
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
        public void Execute(EnabledRefRW<ParticleStartPosition> startPos, ref ParticleMovementOvertime movementOverTime, in LocalTransform localTransform)
        {
            movementOverTime.startPosition = localTransform.Position;
            movementOverTime.movementDirection = math.mul(localTransform.Rotation, movementOverTime.relativeDirection);
            startPos.ValueRW = false;
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
        public float deltaTime;
        public void Execute(in Particle particle, ref ParticleMovementOvertime movementOverTime, ref LocalTransform localTransform)
        {
            ref var curve = ref movementOverTime.data.Value;
            if (movementOverTime.isAdditiveMovement == false)
            {
                localTransform.Position = movementOverTime.startPosition + curve.Evaluate(particle.currentRate) * movementOverTime.movementDirection * movementOverTime.maxDistance;
                return;
            }
            movementOverTime.movementDirection = math.mul(localTransform.Rotation, movementOverTime.relativeDirection);
            localTransform.Position += movementOverTime.movementDirection * deltaTime * curve.Evaluate(particle.currentRate) * movementOverTime.movementDirection * movementOverTime.maxDistance;

        }
    }

    [BurstCompile]
    public partial struct ParticleColorOverTimeJob : IJobEntity
    {
        public void Execute(in Particle particle, in ParticleColorOverTime particleColorOverTime, ref ParticleColor color)
        {
            ref var curve = ref particleColorOverTime.data.Value;
            color.Value = curve.Evaluate(particle.currentRate);
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

            var movementJob = new ParticleMovementOverTimeJob { deltaTime = deltaTime };
            movementJob.ScheduleParallel();

            var colorOvertimeJob = new ParticleColorOverTimeJob();
            colorOvertimeJob.ScheduleParallel();
        }
    }
}
