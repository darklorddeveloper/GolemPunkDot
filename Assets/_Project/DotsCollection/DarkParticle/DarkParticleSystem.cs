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
    public partial struct ParticleSetupSizeOverTimeJob : IJobEntity
    {
        public void Execute(in Particle particle, ref ParticleSizeOverTime sizeOverTime, ref LocalTransform localTransform)
        {
            ref var curve = ref sizeOverTime.data.Value;
            localTransform.Scale = curve.Evaluate(particle.currentRate) * sizeOverTime.maxSize;
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
        
            var setupScaleJob = new ParticleSetupStartSizeJob
            {
                ecb = ecbParallel
            };
            var handle2 = setupScaleJob.ScheduleParallel(handle);
            handle2.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();

            var sizeJob = new ParticleSetupSizeOverTimeJob();
            sizeJob.ScheduleParallel();
        }
    }
}
