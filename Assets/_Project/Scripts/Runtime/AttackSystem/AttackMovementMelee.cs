using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public struct AttackMovementMelee : IComponentData, IEnableableComponent
    {
        public float delayed;
        public bool appliedDamage;
    }
}
