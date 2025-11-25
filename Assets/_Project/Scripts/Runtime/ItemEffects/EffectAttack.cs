using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class EffectAttack : EffectBase
    {
        [GUID] public string attackPrefab;
        public AttackRequestData attackRequestData;
        public override void OnActivate(Entity activator, EntityManager entityManager)
        {
            Entity prefab = SpawnPrefabSystem.GetPrefab(attackPrefab);
            GolemAttackSystem.onGolemAttack(activator, prefab, attackRequestData);
        }
    }
}
