using Unity.Burst;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;

namespace DarkLordGame
{
    [BurstCompile]
    [WithAll(typeof(Attack))]
    public partial struct ChainSpawnerAttackJob : IJobEntity
    {
        public ComponentLookup<Attack> attack;
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunkIndex, Entity e, in Spawner spawner, EnabledRefRW<ChainSpawnerAttack> chainSpawnerAttack)
        {
            if (attack.TryGetComponent(spawner.spawner, out var atk))
            {
                ecb.SetComponent(chunkIndex, e, atk);
                
            }
            chainSpawnerAttack.ValueRW = false;
        }
    }

    public partial struct ChainSpawnerAttackSystem : ISystem
    {
        private ComponentLookup<Attack> atk;
        public void OnCreate(ref SystemState state)
        {
            atk = state.GetComponentLookup<Attack>();
        }

        public void OnUpdate(ref SystemState state)
        {
            atk.Update(ref state);
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
            var job = new ChainSpawnerAttackJob
            {
                attack = atk,
                ecb = ecb.AsParallelWriter()
            };
            var handle = job.ScheduleParallel(state.Dependency);
            handle.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
