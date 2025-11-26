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
                    ecb.SetComponent(chunk, damage, new Damage { damagePosition = pos, attack = attack, target = hit.Entity });
                }
                else
                {
                    var damage = ecb.CreateEntity(chunk, aoeArchetype);
                    ecb.SetComponent(chunk, damage, new AoeDamage { position = pos, attack = attack });

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
    [BurstCompile]
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
            job.ScheduleParallel();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
