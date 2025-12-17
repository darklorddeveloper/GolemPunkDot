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


        public void Execute([ChunkIndexInQuery] int chunk, Entity entity, ref InstanceAIState state,
        in InstanceAIStateSetting settingComponent)
        {
            state.timeSinceStarted += deltaTime;
            ref var setting = ref settingComponent.setting.Value;
            var currentState = InstanceAIStateDataUtility.GetStateData(setting, state.currentStateType);
            if (state.loopCount < currentState.loop && state.timeSinceStarted > currentState.loopInterval)
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
                state.currentStateType = state.inturuptStateType;
                changed = true;
            }

            else if (state.timeSinceStarted >= currentState.stateMaxPeriod)
            {
                changed = true;
                state.currentStateType = currentState.nextState;
            }

            if (changed)
            {
                state.loopCount = 0;
                state.timeSinceStarted = 0;
                currentState = InstanceAIStateDataUtility.GetStateData(setting, state.currentStateType);

                int animationID = (int)currentState.animationIndex;
                ecb.SetComponent(chunk, entity, new CurrentInstanceAnimationIndex { index = animationID });
                ecb.SetComponentEnabled<PlayInstanceAnimation>(chunk, entity, true);
                var stateType = currentState.stateType;
                ecb.SetComponentEnabled<InstanceAIStateFlagIdle>(chunk, entity, stateType == InstanceAIStateType.Idle);
                ecb.SetComponentEnabled<InstanceAIStateFlagMove>(chunk, entity, stateType == InstanceAIStateType.Move);
                ecb.SetComponentEnabled<InstanceAIStateFlagTakeDamage>(chunk, entity, stateType == InstanceAIStateType.TakeDamage);
                ecb.SetComponentEnabled<InstanceAIStateFlagAttack>(chunk, entity, stateType == InstanceAIStateType.Attack);
                ecb.SetComponentEnabled<InstanceAIStateChanged>(chunk, entity, true);
                // ecb.SetComponent(chunk, entity, new TopdownCharacterInput());
            }
        }
    }

    [BurstCompile]
    public partial struct InstanceAIDamageJob : IJobEntity
    {
        public void Execute(in Damage damage, ref InstanceAIState state, ref TopdownCharacterInput input,
        in InstanceAIStateSetting settingComponent)
        {
            state.storedDamage += damage.attack.damage;
            ref var setting = ref settingComponent.setting.Value;
            if (state.storedDamage < setting.interupDamage)
            {
                return;
            }
            state.storedDamage = 0;
            state.isInterupted = true;
            state.inturuptStateType = InstanceAIStateType.TakeDamage;
            input.movement = float3.zero;
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



            //movement
            if (SystemAPI.HasSingleton<PlayerComponent>())
            {
                //reset simple movement
                frameCount++;
                var rand = Random.CreateFromIndex(frameCount);
                var resetJob = new InstanceAISimpleMovementResetJob
                {
                    rand = rand
                };
                handle = resetJob.ScheduleParallel(state.Dependency);
                //movement implementation
                var player = SystemAPI.GetSingleton<PlayerComponent>();
                var wall = SystemAPI.GetSingleton<WallPosition>();
                var movementJob = new InstanceAISimpleMovementJob
                {
                    playerPosition = player.playerPosition,
                    wallPosition = wall.position,
                    deltaTime = deltaTime
                };

                handle = movementJob.ScheduleParallel(handle);

                //attack
                var attackJob = new InstanceAIAttackJob
                {
                    ecb = ecb.AsParallelWriter()
                };
                handle = attackJob.ScheduleParallel(handle);
                handle.Complete();
            }

            var job2 = new InstanceAIDamageJob();
            handle = job2.ScheduleParallel(state.Dependency);
            handle.Complete();

            var job = new InstanceAIStateJob
            {
                deltaTime = deltaTime,
                ecb = ecb.AsParallelWriter()
            };
            handle = job.ScheduleParallel(state.Dependency);
            handle.Complete();


            var disableJob = new DisableInstanceAIStateChangeFlagJob();
            handle = disableJob.ScheduleParallel(state.Dependency);
            handle.Complete();

            ecb.Playback(state.EntityManager);
            ecb.Dispose();


        }
    }
}
