using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct ParticleLifeTimeUpdateJob : IJobEntity
    {
        public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;
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
                    
                    ecb.SetComponentEnabled<Particle>(chunk, e, particleLifeTime.loopCount > 0);
                    return;
                }
            }
            particleLifeTime.timeCount += deltaTime;
            particleLifeTime.currentRate = math.clamp(particleLifeTime.timeCount / particleLifeTime.lifeTime, 0, 1);
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
            var lifetimeJob = new ParticleLifeTimeUpdateJob
            {
                deltaTime = deltaTime,
                ecb = ecb.AsParallelWriter(),
            };
            var handleLifetimeJob = lifetimeJob.ScheduleParallel(state.Dependency);
            handleLifetimeJob.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
