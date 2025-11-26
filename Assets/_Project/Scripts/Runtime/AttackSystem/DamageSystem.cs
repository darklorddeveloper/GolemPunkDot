using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public partial struct DamageJob : IJobEntity
    {
        public ComponentLookup<Death> deathLookup;
        public ComponentLookup<Unit> unitLookup;
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, in Damage damage)
        {
            if (unitLookup.HasComponent(damage.target))
            {
                var unit = unitLookup.GetRefRW(damage.target);
                float dealtDamage = damage.attack.damage;
                float shiled = unit.ValueRW.shield;
                unit.ValueRW.shield -= dealtDamage;
                dealtDamage = unit.ValueRW.shield > 0 ? 0 : (dealtDamage - shiled);
                unit.ValueRW.HP -= dealtDamage;
                if (unit.ValueRW.HP <= 0)
                {
                    if (deathLookup.HasComponent(damage.target))
                    {
                        ecb.SetComponentEnabled<Death>(chunk, damage.target, true);
                    }
                }
            }
        }
    }
    public partial struct DamageSystem : ISystem
    {
        public EntityQuery entityQuery;
        private ComponentLookup<Unit> unitLookup;
        private ComponentLookup<Death> deathLookup;
        public void OnCreate(ref SystemState state)
        {
            unitLookup = state.GetComponentLookup<Unit>();
            deathLookup = state.GetComponentLookup<Death>();
            entityQuery = SystemAPI.QueryBuilder().WithAll<Damage>().Build();

            state.RequireForUpdate<Damage>();
        }
        public void OnUpdate(ref SystemState state)
        {
            deathLookup.Update(ref state);
            unitLookup.Update(ref state);

            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            var job = new DamageJob
            {
                deathLookup = deathLookup,
                unitLookup = unitLookup,
                ecb = ecb.AsParallelWriter(),
            };
            job.ScheduleParallel();
            state.EntityManager.DestroyEntity(entityQuery);
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
