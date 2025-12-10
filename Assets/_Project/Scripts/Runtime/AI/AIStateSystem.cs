using Unity.Burst;
using Unity.Entities;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct AIStateJob : IJobEntity
    {
        public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, Entity e, ref AIState state)
        {
            state.timeSinceStarted += deltaTime;
            if (state.previousState != state.currentState)
            {
                state.previousState = state.currentState;
                state.timeSinceStarted = 0;
                //make enable
                ecb.SetComponentEnabled<AIStateChanged>(chunk, e, true);
            }
        }
    }

    public partial struct AIIdleJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public const int id = (int)InstanceAnimationID.Idle;
        public void Execute([ChunkIndexInQuery] int chunk, Entity entity, EnabledRefRW<AIStateIdle> enabled)
        {
            ecb.SetComponent(chunk, entity, new CurrentInstanceAnimationIndex { index = id });
            ecb.SetComponentEnabled<PlayInstanceAnimation>(chunk, entity, true);
            enabled.ValueRW = false;
        }
    }

    public partial struct AIDamageJob : IJobEntity
    {
        public void Execute(in Damage damage, ref AIState state)
        {
            state.currentState = AIStateType.TakeDamage;
        }
    }

    public partial struct AIStateUnflaggedChangedJob : IJobEntity
    {
        public void Execute(EnabledRefRW<AIStateChanged> changed)
        {
            changed.ValueRW = false;
        }
    }

    [BurstCompile]
    [UpdateBefore(typeof(DamageSystem))]
    public partial struct AIStateSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
            var job = new AIStateJob
            {
                deltaTime = deltaTime,
                ecb = ecb.AsParallelWriter()
            };
            var handle = job.ScheduleParallel(state.Dependency);
            handle.Complete();

            // var job2 = 

            ecb.Playback(state.EntityManager);
            ecb.Dispose();



            var job2 = new AIStateUnflaggedChangedJob();
            handle = job2.ScheduleParallel(state.Dependency);
            handle.Complete();
        }
    }
}
