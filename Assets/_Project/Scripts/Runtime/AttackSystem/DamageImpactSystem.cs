using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct InitDeathImpactJob : IJobEntity
    {
        public void Execute(EnabledRefRW<InitDeathImpact> init, ref DeathImpact deathImpact, in LocalTransform transform, in DeathImpactMovement movmeent)
        {

            var velo = math.max(math.length(deathImpact.velocityDirection) - math.length(deathImpact.sourcePosition - transform.Position) * movmeent.velocityLostPerDistance, 0.0f);
            deathImpact.velocityDirection = math.normalizesafe(deathImpact.velocityDirection, new float3(0, 0, 0)) * velo;
            init.ValueRW = false;
        }
    }

    [BurstCompile]
    public partial struct DeathImpactJob : IJobEntity
    {
        public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, Entity e, ref LocalTransform transform, ref DeathImpactMovement impactMovement, EnabledRefRW<DeathImpactMovement> movementEnable, ref DeathImpact impact)
        {
            var pos = transform.Position;
            var velocity = impact.velocityDirection;
            velocity.y -= impactMovement.gravity * deltaTime;
            if (pos.y <= 0 && velocity.y <= 0)
            {
                velocity.xz = math.lerp(velocity.xz, 0, impactMovement.airDamping * deltaTime);
            }

            pos = pos + velocity * deltaTime;
            if(pos.y > impactMovement.maxHeight)
            {
                velocity.y = math.lerp(velocity.y, 0, impactMovement.damping * deltaTime);
            }
            pos.y = math.clamp(pos.y, 0, impactMovement.maxHeight);

            impact.velocityDirection = velocity;
            transform.Position = pos;
            if (math.lengthsq(velocity.xz) <= 0.1f)
            {
                movementEnable.ValueRW = false;
                ecb.SetComponentEnabled<SafeDestroyComponent>(chunk, e, true);
            }
        }
    }

    [BurstCompile]
    public partial struct DamageImpactSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var setupJob = new InitDeathImpactJob();
            var handle = setupJob.ScheduleParallel(state.Dependency);
            handle.Complete();
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
            float deltaTime = SystemAPI.Time.DeltaTime;
            var job = new DeathImpactJob
            {
                ecb = ecb.AsParallelWriter(),
                deltaTime = deltaTime
            };
            handle = job.ScheduleParallel(state.Dependency);
            handle.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
