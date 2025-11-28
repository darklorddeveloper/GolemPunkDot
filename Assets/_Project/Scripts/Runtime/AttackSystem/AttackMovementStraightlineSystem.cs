using Unity.Entities;
using Unity.Collections;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct AttackMovementStraightlineJob : IJobEntity
    {
        [ReadOnly] public CollisionWorld CollisionWorld;
        [ReadOnly] public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;
        public EntityArchetype damageArchetype;
        public EntityArchetype aoeArchetype;
        public void Execute([ChunkIndexInQuery] int chunk,
        in HitEffect hitEffect,
        in AttackMovementStraightline straightLine,
        in MovementSpeed movementSpeed,
        in Spawner spawner,
        ref Attack attack,
        ref LocalTransform transform
        )
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
                var rot = hitEffect.inheritRotation ? transform.Rotation : quaternion.identity;
                ecb.SetComponent(chunk, e, new LocalTransform
                {
                    Position = hit.Position,
                    Rotation = rot,
                    Scale = attack.aoeRange > 1 ? attack.aoeRange : 1.0f
                });
                // if (spawner.spawner != Entity.Null)
                //     ecb.SetComponent(chunk, e, spawner);
                //create damage entity. --- if no aoe just damage
                if (attack.aoeRange <= 1.0f)
                {
                    var damage = ecb.CreateEntity(chunk, damageArchetype);
                    ecb.SetComponent(chunk, damage, new Damage { damagePosition = pos, attack = attack, target = hit.Entity });
                }
                else
                {
                    var damage = ecb.CreateEntity(chunk, aoeArchetype);
                    ecb.SetComponent(chunk, damage, new AoeDamage { position = pos, attack = attack });

                }
                if (attack.bounce <= 0)
                {
                    ecb.SetComponentEnabled<SafeDestroyComponent>(chunk, e, true);
                }
            }
            else
            {
                transform.Position = target;
                Debug.DrawLine(target, target + new float3(0, 4, 0), Color.red);
            }

        }
    }
    [BurstCompile]
    [UpdateAfter(typeof(AttackSystem))]
    public partial struct AttackMovementStraightlineSystem : ISystem
    {
        private EntityArchetype damageArchetype;
        private EntityArchetype aoeArchetype;

        public void OnCreate(ref SystemState state)
        {
            damageArchetype = state.EntityManager.CreateArchetype(typeof(Damage));
            aoeArchetype = state.EntityManager.CreateArchetype(typeof(AoeDamage));
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            float deltaTime = SystemAPI.Time.DeltaTime;
            var physics = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            var cw = physics.CollisionWorld; // a value type, safe to capture
            var job = new AttackMovementStraightlineJob
            {
                ecb = ecb.AsParallelWriter(),
                aoeArchetype = aoeArchetype,
                damageArchetype = damageArchetype,
                deltaTime = deltaTime,
                CollisionWorld = cw
            };
            var handle = job.ScheduleParallel(state.Dependency);
            handle.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
