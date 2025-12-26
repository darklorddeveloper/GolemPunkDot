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
        public void Execute([ChunkIndexInQuery] int chunk,
        Entity entity, ref Unit unit, ref Damage damage,
        ref LocalTransform localTransform,
        EnabledRefRW<Damage> damageEnable, in DeathEffect effect)
        {
            float dealtDamage = damage.damageToken;
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
                        ecb.SetComponent(chunk, fx, new DeathImpact
                        {
                            impactDirection = damage.impactDirection,
                            sourcePosition = damage.damageSourcePosition,
                        });
                        ecb.SetComponent(chunk, fx, new DeathImpactDamage { damage = damage.damageToken });
                    }
                    ecb.SetComponentEnabled<SafeDestroyComponent>(chunk, entity, true);
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
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Damage>();
        }
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            var job = new DamageJob
            {
                ecb = ecb.AsParallelWriter(),
            };
            job.ScheduleParallel();
            // var handle = job.ScheduleParallel(state.Dependency);
            // handle.Complete();
            // ecb.Playback(state.EntityManager);
            // ecb.Dispose();
        }
    }
}
