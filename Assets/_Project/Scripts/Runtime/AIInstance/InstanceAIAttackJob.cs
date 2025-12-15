using Unity.Burst;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Transforms;

namespace DarkLordGame
{


    [BurstCompile]
    [WithAll(typeof(InstanceAIStateFlagAttack), typeof(InstanceAIStateChanged))]
    public partial struct InstanceAIAttackResetJob : IJobEntity
    {
        public void Execute(ref InstanceAIAttack attack)
        {
            attack.attacked = false;
        }
    }

    [BurstCompile]
    [WithAll(typeof(InstanceAIStateFlagAttack))]
    public partial struct InstanceAIAttackJob : IJobEntity
    {
        public void Execute(ref InstanceAIState state, ref AttackRequestData request,
        EnabledRefRW<AttackRequestData> enableRequest, ref InstanceAIAttack attack,
        in LocalTransform transform)
        {
            if (attack.attacked == false && state.timeSinceStarted > attack.delayed)
            {
                enableRequest.ValueRW = true;
                request.position = transform.Position;
                request.rotation = transform.Rotation;
                attack.attacked = true;
            }
        }
    }
}
