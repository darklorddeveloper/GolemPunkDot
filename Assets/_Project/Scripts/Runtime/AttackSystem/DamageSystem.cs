using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct DamageJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, Entity entity, ref Unit unit, ref Damage damage,
        ref LocalTransform localTransform,
         EnabledRefRW<Damage> damageEnable, in DeathEffect effect)
        {
            float dealtDamage = damage.attack.damage;
            float shield = unit.shield;
            unit.shield -= dealtDamage;
            unit.shield = math.max(0, unit.shield);
            dealtDamage = unit.shield > 0 ? 0 : (dealtDamage - shield);
            unit.HP -= dealtDamage;
            if (unit.HP <= 0)
            {
                if (unit.canDeath)
                {
                    ecb.SetComponentEnabled<Death>(chunk, entity, true);
                    if (effect.prefab != Entity.Null)
                    {
                        var fx = ecb.Instantiate(chunk, effect.prefab);
                        ecb.SetComponent(chunk, fx, localTransform);
                        var diff = math.normalizesafe(localTransform.Position - damage.damageSourcePosition, new float3(0, 1, 0));
                        ecb.SetComponent(chunk, fx, new DeathImpact
                        {
                            velocityDirection = diff * damage.attack.pushPower + new float3(0.0f, damage.attack.riftPower, 0.0f),
                            sourcePosition = damage.damageSourcePosition,
                        });
                        ecb.SetComponent(chunk, fx, new DeathImpactDamage { attack = damage.attack });
                        ecb.SetComponentEnabled<DealDeathImpactDamage>(chunk, fx, damage.attack.canDealDeathImpact);
                    }
                }
                else
                {
                    unit.HP = unit.MaxHP;
                }
            }
            damageEnable.ValueRW = false;
        }
    }

    [BurstCompile]
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
