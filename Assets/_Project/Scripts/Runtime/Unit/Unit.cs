using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public struct Unit : IComponentData
    {
        public bool canDeath;
        public float HP;
        public float MaxHP;
        public float shield;//always temp
        public float attack;
        public float criticalChance;//50%
        public float criitcalDamage;//1.5f
        public float bonusAoeDamageRate;
        public float bonusAoeRange;
    }

    public struct IsDeath : IComponentData, IEnableableComponent
    {
    }

    public struct CanDeath : IComponentData
    {
        
    }

    public struct CooldownSpeed : IComponentData//only player
    {
        public float cooldownSpeed;
    }

    public struct AOE : IComponentData
    {
        public ushort bonusAOEDamage;
        public ushort bonusAoeRange;
    }

    

    [System.Serializable]
    public struct ChangeMovementSpeed : IComponentData, IEnableableComponent
    {

    }
}
