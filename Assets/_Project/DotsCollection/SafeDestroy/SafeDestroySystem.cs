using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Burst;
using UnityEngine;

namespace DarkLordGame
{
    // [BurstCompile]
    public partial struct SafeDestroyCountDownJob : IJobEntity
    {
        public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, ref SafeDestroyComponent component)
        {
            component.period -= deltaTime;
            if (component.period <= 0)
                ecb.DestroyEntity(chunkIndex, entity);
        }
    }

    public partial struct SafeDestroySystem : ISystem
    {
        private EntityQuery query;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SafeDestroyComponent>();
            query = SystemAPI.QueryBuilder().WithAll<SafeDestroyComponent>().Build();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
            var job = new SafeDestroyCountDownJob
            {
                ecb = ecb.AsParallelWriter(),
                deltaTime = deltaTime
            };
            var dependency = job.ScheduleParallel(state.Dependency);
            dependency.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
