using Unity.Entities;
using Unity.Collections;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;

namespace DarkLordGame
{
    public partial struct AttackMovementStraightlineJob : IJobEntity
    {
        [ReadOnly] public CollisionWorld CollisionWorld;
        [ReadOnly] public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;
        public EntityArchetype damageArchetype;
        public EntityArchetype aoeArchetype;
        public void Execute([ChunkIndexInQuery] int chunk,
        ref Attack attack,
        in HitEffect hitEffect,
        in AttackMovementStraightline straightLine,
        in MovementSpeed movementSpeed,
        ref LocalTransform transform,
        in Spawner spawner,
        EnabledRefRW<SafeDestroyComponent> safeDestroy)
        {
            var pos = transform.Position;
            var forward = transform.Forward();
            float3 target = pos + forward * deltaTime * movementSpeed.value * movementSpeed.multiplier;
            var raycastInput = new RaycastInput
            {
                Start = pos,
                End = target,
                Filter = new CollisionFilter
                {
                    BelongsTo = attack.belongToLayer,
                    CollidesWith = attack.hitLayer,
                }
            };
            if (CollisionWorld.CastRay(raycastInput, out var hit))
            {
                attack.bounce--;
                transform.Position = hit.Position + math.reflect(forward, hit.SurfaceNormal) * straightLine.offsetFromWall;
                transform.Rotation = quaternion.LookRotation(hit.SurfaceNormal, new float3(0, 1, 0));
                var e = ecb.Instantiate(chunk, hitEffect.entity);
                ecb.SetComponent(chunk, e, new LocalTransform
                {
                    Position = hit.Position,
                    Rotation = transform.Rotation,
                    Scale = attack.aoeRange > 1 ? attack.aoeRange : 1.0f
                });

                ecb.SetComponent(chunk, e, new Spawner { spawner = spawner.spawner });

                //create damage entity. --- if no aoe just damage
                if (attack.aoeRange <= 1.0f)
                {
                    var damage = ecb.CreateEntity(chunk, damageArchetype);
                    ecb.SetComponent(chunk, damage, new Damage { attack = attack, target = hit.Entity });
                }
                else
                {
                    var damage = ecb.CreateEntity(chunk, aoeArchetype);
                    ecb.SetComponent(chunk, damage, new AoeDamage{position = pos, attack = attack });
                    
                }
                // aoe component first then do the damage from aoe system

                if (attack.bounce <= 0)
                {
                    safeDestroy.ValueRW = true;
                }
            }
            else
            {
                transform.Position = target;
            }

        }
    }
    public partial struct AttackMovementStraightlineSystem : ISystem
    {
    }
}
