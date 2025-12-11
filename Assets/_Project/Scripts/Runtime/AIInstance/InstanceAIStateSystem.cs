using System.Diagnostics;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct InstanceAIStateJob : IJobEntity
    {
        public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, Entity entity, ref InstanceAIState state)
        {
            state.timeSinceStarted += deltaTime;

            if (state.loopCount < state.currentStateData.loop && state.timeSinceStarted > state.currentStateData.loopInterval)
            {
                state.timeSinceStarted = 0;
                state.loopCount++;
                ecb.SetComponentEnabled<PlayInstanceAnimation>(chunk, entity, true);
                ecb.SetComponentEnabled<InstanceAIStateChanged>(chunk, entity, true);
                return;
            }

            bool changed = false;
            if (state.isInterupted)//manual active or take damage
            {
                state.isInterupted = false;
                state.currentStateData = state.interuptState;
                changed = true;
            }
            else if (state.timeSinceStarted >= state.currentStateData.stateMaxPeriod)
            {
                changed = true;

                switch (state.currentStateData.nextState)
                {
                    case InstanceAIStateType.Idle:
                        state.currentStateData = state.idleState;
                        break;
                    case InstanceAIStateType.Move:
                        state.currentStateData = state.moveState;
                        break;
                    case InstanceAIStateType.Attack:
                        state.currentStateData = state.attackState;
                        break;
                    case InstanceAIStateType.TakeDamage:
                        state.currentStateData = state.takeDamageState;
                        break;
                }
            }
            if (changed)
            {
                state.loopCount = 0;
                state.timeSinceStarted = 0;

                int animationID = (int)state.currentStateData.animationIndex;
                ecb.SetComponent(chunk, entity, new CurrentInstanceAnimationIndex { index = animationID });
                ecb.SetComponentEnabled<PlayInstanceAnimation>(chunk, entity, true);
                var stateType = state.currentStateData.stateType;
                ecb.SetComponentEnabled<InstanceAIStateFlagIdle>(chunk, entity, stateType == InstanceAIStateType.Idle);
                ecb.SetComponentEnabled<InstanceAIStateFlagMove>(chunk, entity, stateType == InstanceAIStateType.Move);
                ecb.SetComponentEnabled<InstanceAIStateFlagTakeDamage>(chunk, entity, stateType == InstanceAIStateType.TakeDamage);
                ecb.SetComponentEnabled<InstanceAIStateFlagAttack>(chunk, entity, stateType == InstanceAIStateType.Attack);
                ecb.SetComponentEnabled<InstanceAIStateChanged>(chunk, entity, true);
                ecb.SetComponent(chunk, entity, new TopdownCharacterInput());

            }
        }
    }

    [BurstCompile]
    public partial struct InstanceAIDamageJob : IJobEntity
    {
        public void Execute(in Damage damage, ref InstanceAIState state)
        {
            state.storedDamage += damage.attack.damage;
            if (state.storedDamage < state.interupDamage)
            {
                return;
            }
            state.storedDamage = 0;
            state.isInterupted = true;
            state.interuptState = state.takeDamageState;
        }
    }

    [BurstCompile]
    public partial struct DisableInstanceAIStateChangeFlagJob : IJobEntity
    {
        public void Execute(EnabledRefRW<InstanceAIStateChanged> change)
        {
            change.ValueRW = false;
        }
    }

    [BurstCompile]
    [UpdateBefore(typeof(DamageReactionSystem))]
    public partial struct InstanceAIStateSystem : ISystem
    {
        private uint frameCount;
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
            //damage job first
            var damageJob = new InstanceAIDamageJob();
            var handle = damageJob.ScheduleParallel(state.Dependency);
            handle.Complete();

            var job = new InstanceAIStateJob
            {
                deltaTime = deltaTime,
                ecb = ecb.AsParallelWriter()
            };
            handle = job.ScheduleParallel(state.Dependency);
            handle.Complete();

            var job2 = new InstanceAIDamageJob();
            handle = job2.ScheduleParallel(state.Dependency);
            handle.Complete();

            //movement
            if (SystemAPI.HasSingleton<PlayerComponent>())
            {
                //reset movement
                frameCount++;
                var rand = Random.CreateFromIndex(frameCount);
                var resetJob = new InstanceAIMovementResetJob
                {
                    rand = rand
                };
                handle = resetJob.ScheduleParallel(state.Dependency);
                handle.Complete();
                //movement implementation
                var player = SystemAPI.GetSingleton<PlayerComponent>();
                var wall = SystemAPI.GetSingleton<WallPosition>();
                var movementJob = new InstanceAIMovementJob
                {
                    playerPosition = player.playerPosition,
                    wallPosition = wall.position
                };

                handle = movementJob.ScheduleParallel(state.Dependency);
                handle.Complete();
            }

            //attack

            var disableJob = new DisableInstanceAIStateChangeFlagJob();
            handle = disableJob.ScheduleParallel(state.Dependency);
            handle.Complete();

            ecb.Playback(state.EntityManager);
            ecb.Dispose();


        }
    }
}
