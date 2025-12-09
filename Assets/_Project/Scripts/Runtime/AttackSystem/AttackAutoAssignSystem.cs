using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct AutoAssignAttackerJob : IJobEntity
    {
        public void Execute(Entity entity, ref AttackRequestData attackRequestData, EnabledRefRW<AttackRequestAutoAssignAttacker> enableAssign)
        {
            attackRequestData.attacker = entity;
            enableAssign.ValueRW = false;
        }
    }

    public partial struct AutoAssignAttackPrefabJob : IJobEntity
    {
        public void Execute(ref AttackRequestData attackRequestData, ref AttackRequestAutoAssignPrefab prefab, EnabledRefRW<AttackRequestAutoAssignPrefab> enablePrefab)
        {
            attackRequestData.prefab = prefab.prefab;
            enablePrefab.ValueRW = false;
        }
    }

    [BurstCompile]
    public partial struct AttackAutoAssignSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var job1 = new AutoAssignAttackerJob();
            var handle = job1.ScheduleParallel(state.Dependency);
            var job2 = new AutoAssignAttackPrefabJob();
            var handle2 = job2.ScheduleParallel(handle);
            state.Dependency = handle2;
        }
    }
}
