using Unity.Burst;
using Unity.Entities;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct DamageReactionJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, Entity entity, in Damage damage, DynamicBuffer<InstanceAnimation> damageReactions, ref CurrentInstanceAnimationIndex index)
        {
            index.index = 3;
            ecb.SetComponentEnabled<DamageTime>(chunk, damageReactions[index.index].target, true);
            ecb.SetComponentEnabled<PlayInstanceAnimation>(chunk, entity,true );
        }
    }

    [BurstCompile]
    [UpdateBefore(typeof(InstanceAnimationSystem))]
    [UpdateBefore(typeof(DamageSystem))]
    public partial struct DamageReactionSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
            var job = new DamageReactionJob
            {
                ecb = ecb.AsParallelWriter(),
            };
            var handle = job.ScheduleParallel(state.Dependency);
            handle.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
