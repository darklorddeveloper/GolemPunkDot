using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public partial struct AttackAutoAssignSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (attackdata, attackerAssign, entity) in SystemAPI.Query<RefRW<AttackRequestData>, EnabledRefRW<AttackRequestAutoAssignAttacker>>().WithEntityAccess())
            {
                attackdata.ValueRW.attacker = entity;
                attackerAssign.ValueRW = false;
            }

            foreach (var (attackdata, prefabAssign, entity) in SystemAPI.Query<RefRW<AttackRequestData>, AttackRequestAutoAssignPrefab>().WithEntityAccess())
            {
                attackdata.ValueRW.prefab = prefabAssign.prefab;
                state.EntityManager.SetComponentEnabled<AttackRequestAutoAssignPrefab>(entity, false);
            }
        }
    }
}
