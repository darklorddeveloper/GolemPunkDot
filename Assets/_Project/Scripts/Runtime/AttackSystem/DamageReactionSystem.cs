using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public partial struct DamageReactionJob : IJobEntity
    {
        public float time;
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, DynamicBuffer<DamageReaction> damageReactions)
        {
            for (int i = 0, length = damageReactions.Length; i < length; i++)
            {
                ecb.SetComponent(chunk, damageReactions[i].target, new DamageTime { Value = time });
            }
        }
    }

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
