using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Effect/ Attack")]
    public class EffectAttack : EffectBase
    {
        [GUID] public string attackPrefab;
        public GolemAttachPoint attachPoint = GolemAttachPoint.AttackForwardPoint;
        public AttackRequestData attackRequestData;
        public bool canChainAttack = false;
        public override void OnActivate(Entity activator, EntityManager entityManager)
        {
            GolemAttackSystem.onGolemAttack(activator, attackPrefab, attachPoint, attackRequestData, canChainAttack);
        }
    }
}
