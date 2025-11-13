using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    [UpdateAfter(typeof(AttackAutoAssignSystem))]
    public partial struct AttackSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            // state.EntityManager.SetComponentEnabled()
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            var deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var (attackRW, attackEntity) in SystemAPI.Query<RefRW<AttackRequestData>>().WithEntityAccess())
            {
                var attack = attackRW.ValueRO;
                attackRW.ValueRW.loopTimeCount += deltaTime;

                if (attack.loopCasted >= 1 && attack.loopTimeCount < attack.loopCastInterval)
                {
                    continue;
                }
                attackRW.ValueRW.loopTimeCount = 0;

                var rot = attack.rotation;
                var pos = attack.position;

                if (attack.extraSplit > 0)
                {
                    int split = attack.extraSplit + 1;
                    float startAngle = -attack.splitAngle * (split - 1.0f) / 2.0f;

                    //instantiate
                    NativeArray<Entity> entities = new NativeArray<Entity>(split, Allocator.Temp);
                    ecb.Instantiate(attack.prefab, entities);
                    for (int i = 0; i <= split; i++)
                    {
                        //set position and split
                        var extraRot = quaternion.Euler(0, 0, startAngle + i * attack.splitAngle);
                        var targetRot = math.mul(rot, extraRot);
                        ecb.SetComponent(entities[i], new LocalTransform { Position = pos, Rotation = targetRot, Scale = 1.0f });
                        ecb.SetComponent(entities[i], new Spawner { spawner = attack.attacker });
                        //calculate attack and set here
                    }
                    entities.Dispose();
                }
                else
                {
                    var e = ecb.Instantiate(attack.prefab);
                    ecb.SetComponent(e, new LocalTransform { Position = pos, Rotation = rot, Scale = 1.0f });
                    ecb.SetComponent(e, new Spawner { spawner = attack.attacker });
                }

                attackRW.ValueRW.loopCasted++;
                if (attackRW.ValueRW.loopCasted >= attackRW.ValueRW.loopCast)
                {
                    ecb.SetComponentEnabled<AttackRequestData>(attackEntity, false);
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
