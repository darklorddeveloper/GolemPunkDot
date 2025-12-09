using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DarkLordGame
{

    [BurstCompile]
    public partial struct MeleeAttackJob : IJobEntity
    {
        public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;
        public EntityArchetype aoeArchetype;

        public void Execute([ChunkIndexInQuery] int chunk, Entity entity,
        in LocalTransform transform,
        in Attack attack,
         ref AttackMovementMelee melee,
         EnabledRefRW<AttackMovementMelee> meleeEnable,
         ref Spawner spawner)
        {
            melee.delayed -= deltaTime;
            if (melee.delayed <= 0)
            {
                meleeEnable.ValueRW = false;
                var damage = ecb.CreateEntity(chunk, aoeArchetype);
                ecb.SetComponent(chunk, damage, new AoeDamage { position = transform.Position, forward = transform.Forward(), attack = attack });
                ecb.SetComponentEnabled<SafeDestroyComponent>(chunk, entity, true);
            }
        }
    }
    [BurstCompile]
    public partial struct AttackMovementMeleeSystem : ISystem
    {
        private EntityArchetype aoeArchetype;
        public void OnCreate(ref SystemState state)
        {
            aoeArchetype = state.EntityManager.CreateArchetype(typeof(AoeDamage));
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecbSystem = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);
            float deltaTime = SystemAPI.Time.DeltaTime;
            var job = new MeleeAttackJob
            {
                aoeArchetype = aoeArchetype,
                ecb = ecb.AsParallelWriter(),
                deltaTime = deltaTime
            };
            job.ScheduleParallel();

        }
    }
}
