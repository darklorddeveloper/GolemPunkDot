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
        Entity entity,
        in HitEffect hitEffect,
        ref AttackMovementStraightline straightLine,
        EnabledRefRW<AttackMovementStraightline> enableStraightline,
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
                if (hitEffect.entity != Entity.Null)
                {
                    var e = ecb.Instantiate(chunk, hitEffect.entity);
                    var rot = hitEffect.inheritRotation ? transform.Rotation : quaternion.identity;
                    ecb.SetComponent(chunk, e, new LocalTransform
                    {
                        Position = hit.Position,
                        Rotation = rot,
                        Scale = attack.aoeRange > 1 ? attack.aoeRange : 1.0f
                    });
                }
                if (attack.aoeRange <= 1.0f)
                {
                    ecb.SetComponent(chunk, hit.Entity, new Damage { damagePosition = hit.Position, damageSourcePosition = pos, attack = attack });
                    ecb.SetComponentEnabled<Damage>(chunk, hit.Entity, true);
                }
                else
                {
                    var damage = ecb.CreateEntity(chunk, aoeArchetype);
                    ecb.SetComponent(chunk, damage, new AoeDamage { position = pos, forward = forward, attack = attack });

                }
                if (attack.bounce <= 0)
                {
                    enableStraightline.ValueRW = false;
                    ecb.SetComponentEnabled<SafeDestroyComponent>(chunk, entity, true);
                }
            }
            else
            {
                transform.Position = target;
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
            var ecbSystem = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);
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
            job.ScheduleParallel();
        }
    }
}
