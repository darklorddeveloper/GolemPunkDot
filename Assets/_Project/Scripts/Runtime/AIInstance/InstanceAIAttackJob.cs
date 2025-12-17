using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DarkLordGame
{



    [BurstCompile]
    [WithAll(typeof(InstanceAIStateFlagAttack))]
    public partial struct InstanceAIAttackJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, Entity e, ref InstanceAIState state,
        ref InstanceAIAttack attack,
        EnabledRefRW<InstanceAIStateFlagAttack> attackEnable)
        {
            if (state.timeSinceStarted > attack.delayed)
            {
                attackEnable.ValueRW = false;

                ecb.SetComponentEnabled<SpawnAttackRequest>(chunk, e, true);
            }
        }
    }
}
