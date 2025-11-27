using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    //calculate damage pass to attack
    [UpdateAfter(typeof(AttackAutoAssignSystem))]
    public partial struct AttackSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            // state.EntityManager.SetComponentEnabled()
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            var deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var (attackRequestRW, unit, attackEntity) in SystemAPI.Query<RefRW<AttackRequestData>, Unit>().WithEntityAccess())
            {
                var attackRequest = attackRequestRW.ValueRO;
                attackRequestRW.ValueRW.loopTimeCount += deltaTime;
                if (attackRequest.loopCasted >= 1 && attackRequest.loopTimeCount < attackRequest.loopCastInterval)
                {
                    continue;
                }
                attackRequestRW.ValueRW.loopTimeCount = 0;

                float baseDamage = (unit.attack + unit.tempAttack + attackRequest.bonusDamage) * (1 + unit.tempAttackMultiplier + attackRequest.attackDamageMultipler);
                float critDamage = unit.criitcalDamage + unit.tempCriticalDamage;
                float critChance = unit.criticalChance + unit.tempCriticalChance;
                var attack = new Attack
                {
                    damage = baseDamage,
                    attackProperty = attackRequest.attackProperty,
                    propertyValue = attackRequest.propertyValue,
                    aoeDamage = baseDamage * (unit.bonusAoeDamageRate + attackRequest.aoeDamageRate),
                    aoeRange = unit.bonusAoeRange + attackRequest.aoeRange,
                    criticalDamage = critDamage + attackRequest.extraCritDamage,
                    criticalChance = critChance + attackRequest.extracriticalChance,
                };

                var rot = attackRequest.rotation;
                var pos = attackRequest.position;

                if (attackRequest.extraSplit > 0)
                {
                    int split = attackRequest.extraSplit + 1;
                    float startAngle = -attackRequest.splitAngle * (split - 1.0f) / 2.0f;

                    //instantiate
                    NativeArray<Entity> entities = new NativeArray<Entity>(split, Allocator.Temp);
                    ecb.Instantiate(attackRequest.prefab, entities);
                    for (int i = 0; i <= split; i++)
                    {
                        //set position and split
                        var extraRot = quaternion.Euler(0, 0, startAngle + i * attackRequest.splitAngle);
                        var targetRot = math.mul(rot, extraRot);
                        ecb.SetComponent(entities[i], new LocalTransform { Position = pos, Rotation = targetRot, Scale = 1.0f });
                        ecb.SetComponent(entities[i], new Spawner { spawner = attackRequest.attacker });
                        ecb.SetComponent(entities[i], attack);
                    }
                    entities.Dispose();
                }
                else
                {
                    var e = ecb.Instantiate(attackRequest.prefab);
                    ecb.SetComponent(e, new LocalTransform { Position = pos, Rotation = rot, Scale = 1.0f });
                    ecb.SetComponent(e, new Spawner { spawner = attackRequest.attacker });
                    ecb.SetComponent(e, attack);
                }

                attackRequestRW.ValueRW.loopCasted++;
                if (attackRequestRW.ValueRW.loopCasted >= attackRequestRW.ValueRW.loopCast)
                {
                    ecb.SetComponentEnabled<AttackRequestData>(attackEntity, false);
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
