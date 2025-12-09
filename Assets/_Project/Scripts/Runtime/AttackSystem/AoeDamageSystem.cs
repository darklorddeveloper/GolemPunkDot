using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct AoeDamageSystem : ISystem
    {
        public EntityQuery entityQuery;
        public EntityArchetype damageArchetype;
        public void OnCreate(ref SystemState state)
        {
            entityQuery = SystemAPI.QueryBuilder().WithAll<AoeDamage>().Build();
            damageArchetype = state.EntityManager.CreateArchetype(typeof(Damage));
            state.RequireForUpdate<AoeDamage>();
        }
        public void OnUpdate(ref SystemState state)
        {
            var components = entityQuery.ToComponentDataArray<AoeDamage>(Allocator.Temp);
            var physics = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            var collisionWorld = physics.CollisionWorld;
            for (int i = 0, length = components.Length; i < length; i++)
            {
                NativeList<DistanceHit> hits = new NativeList<DistanceHit>(Allocator.Temp);
                var attack = components[i].attack;
                collisionWorld.OverlapSphere(components[i].position, attack.aoeRange, ref hits,
                new CollisionFilter
                {
                    BelongsTo = attack.belongToLayer,
                    CollidesWith = attack.hitLayer
                });
                if (hits.Length > 0)
                {
                    // NativeArray<Entity> damageEntities = new NativeArray<Entity>(hits.Length, Allocator.Temp);
                    // state.EntityManager.CreateEntity(damageArchetype, damageEntities);
                    for (int j = 0, numbers = hits.Length; j < numbers; j++)
                    {
                        var e = hits[j].Entity;
                        if (attack.useLimitedAngle)
                        {
                            var dis = math.normalize(hits[j].Position - components[i].position);
                            var dot = math.dot(dis, components[i].forward);
                            if (dot < attack.limitedDot)
                            {
                                continue;
                            }
                        }
                        if (state.EntityManager.HasComponent<Damage>(e))
                        {
                            state.EntityManager.SetComponentData(e, new Damage
                            {
                                attack = attack,
                                damagePosition = hits[j].Position,
                                damageSourcePosition = components[i].position,
                            });
                            state.EntityManager.SetComponentEnabled<Damage>(e, true);
                        }
                    }
                }
                hits.Dispose();
            }
            state.EntityManager.DestroyEntity(entityQuery);
        }
    }
}
