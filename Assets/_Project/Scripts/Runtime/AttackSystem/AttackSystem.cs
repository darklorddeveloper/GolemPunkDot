using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{

    public partial struct AttackJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public float deltaTime;
        public void Execute([ChunkIndexInQuery] int chunk, Entity entity, ref AttackRequestData attackRequest, in Unit unit)
        {

            attackRequest.loopTimeCount += deltaTime;
            if (attackRequest.loopCasted >= 1 && attackRequest.loopTimeCount < attackRequest.loopCastInterval)
            {
                return;
            }
            attackRequest.loopTimeCount = 0;

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
                NativeArray<Entity> entities = new NativeArray<Entity>(split, Allocator.TempJob);
                ecb.Instantiate(chunk, attackRequest.prefab, entities);
                for (int i = 0; i <= split; i++)
                {
                    //set position and split
                    var extraRot = quaternion.Euler(0, 0, startAngle + i * attackRequest.splitAngle);
                    var targetRot = math.mul(rot, extraRot);
                    ecb.SetComponent(chunk, entities[i], new LocalTransform { Position = pos, Rotation = targetRot, Scale = 1.0f });
                    ecb.SetComponent(chunk, entities[i], new Spawner { spawner = attackRequest.attacker });
                    ecb.SetComponent(chunk, entities[i], attack);
                }
                entities.Dispose();
            }
            else
            {
                var e = ecb.Instantiate(chunk, attackRequest.prefab);
                ecb.SetComponent(chunk, e, new LocalTransform { Position = pos, Rotation = rot, Scale = 1.0f });
                ecb.SetComponent(chunk, e, new Spawner { spawner = attackRequest.attacker });
                ecb.SetComponent(chunk, e, attack);
            }

            attackRequest.loopCasted++;
            if (attackRequest.loopCasted >= attackRequest.loopCast)
            {
                ecb.SetComponentEnabled<AttackRequestData>(chunk, entity, false);
            }
        }
    }
    //calculate damage pass to attack
    [UpdateAfter(typeof(AttackAutoAssignSystem))]
    public partial struct AttackSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            float deltaTime = SystemAPI.Time.DeltaTime;
            var job = new AttackJob
            {
                ecb = ecb.AsParallelWriter(),
                deltaTime = deltaTime
            };
            var handle = job.ScheduleParallel(state.Dependency);
            handle.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
