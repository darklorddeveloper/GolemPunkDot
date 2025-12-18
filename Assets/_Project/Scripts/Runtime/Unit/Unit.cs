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
    }

    public struct IsDeath : IComponentData, IEnableableComponent
    {
    }

    public struct CanDeath : IComponentData
    {

    }

    [System.Serializable]
    public struct Crit : IComponentData
    {
        public float criticalChance;
        public float criitcalDamage;
    }

    [System.Serializable]
    public struct CooldownSpeed : IComponentData//only player
    {
        public float cooldownSpeed;
    }

    [System.Serializable]
    public struct AOE : IComponentData
    {
        public ushort bonusAOEDamageRate;
        public ushort bonusAoeRange;
    }
}
