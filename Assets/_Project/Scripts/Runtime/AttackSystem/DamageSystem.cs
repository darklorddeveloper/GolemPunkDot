using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public partial struct DamageJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, Entity entity, ref Unit unit, ref Damage damage, EnabledRefRW<Damage> damageEnable)
        {
            float dealtDamage = damage.attack.damage;
            float shiled = unit.shield;
            unit.shield -= dealtDamage;
            dealtDamage = unit.shield > 0 ? 0 : (dealtDamage - shiled);
            unit.HP -= dealtDamage;
            if (unit.HP <= 0)
            {
                if (unit.canDeath)
                {
                    ecb.SetComponentEnabled<Death>(chunk, entity, true);
                }
                else
                {
                    unit.HP = unit.MaxHP;
                }
            }
            damageEnable.ValueRW = false;
        }
    }
    public partial struct DamageSystem : ISystem
    {
        public EntityQuery entityQuery;
        public void OnCreate(ref SystemState state)
        {
            entityQuery = SystemAPI.QueryBuilder().WithAll<Damage>().Build();

            state.RequireForUpdate<Damage>();
        }
        public void OnUpdate(ref SystemState state)
        {

            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            var job = new DamageJob
            {
                ecb = ecb.AsParallelWriter(),
            };
            job.ScheduleParallel();
            state.EntityManager.DestroyEntity(entityQuery);
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
