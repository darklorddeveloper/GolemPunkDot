using Unity.Burst;
using Unity.Entities;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct DamageReactionJob : IJobEntity
    {
        public float time;
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, in Damage damage, DynamicBuffer<InstanceAnimation> damageReactions, in CurrentInstanceAnimationIndex index)
        {
            ecb.SetComponent(chunk, damageReactions[index.index].target, new DamageTime { Value = time });
        }
    }

    [BurstCompile]
    public partial struct DamageReactionSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
            var time = (float)SystemAPI.Time.ElapsedTime;
            var job = new DamageReactionJob
            {
                ecb = ecb.AsParallelWriter(),
                time = time
            };
            var handle = job.ScheduleParallel(state.Dependency);
            handle.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
