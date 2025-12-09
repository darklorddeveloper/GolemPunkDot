using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct PlayInstanceAnimationJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, EnabledRefRW<PlayInstanceAnimation> enableAnimation, DynamicBuffer<InstanceAnimation> instanceAnimations, in CurrentInstanceAnimationIndex currentAnimationIndex)
        {
            for (int i = 0, length = instanceAnimations.Length; i < length; i++)
            {
                bool isTarget = i == currentAnimationIndex.index;
                ecb.SetEnabled(chunk, instanceAnimations[i].target, isTarget);
                ecb.SetComponentEnabled<StartTime>(chunk, instanceAnimations[i].target, true);
            }
            enableAnimation.ValueRW = false;
        }
    }

    [BurstCompile]
    public partial struct InstanceAnimationSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayInstanceAnimation>();
        }
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
            var job = new PlayInstanceAnimationJob
            {
                ecb = ecb.AsParallelWriter()
            };
            var handle = job.ScheduleParallel(state.Dependency);
            handle.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
