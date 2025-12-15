using System.Diagnostics;
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
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, Entity e, ref InstanceAIState state,
        ref AttackRequestTransform request,
        ref InstanceAIAttack attack,
        in LocalTransform transform)
        {
            if (attack.attacked == false && state.timeSinceStarted > attack.delayed)
            {
                request.position = transform.Forward() * request.forwardOffset + transform.Position;
                request.rotation = transform.Rotation;
                attack.attacked = true;
                ecb.SetComponentEnabled<AttackRequestData>(chunk, e, true);
            }
        }
    }
}
