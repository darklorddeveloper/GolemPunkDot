using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public struct TempStat : IComponentData, IEnableableComponent
    {
        public Entity target;
        public float tempShield;
        public float tempAttack;
        public float tempAttackMultiplier;
        public float tempCriticalChance;
        public float tempCriticalDamage;
        public float bonusAoeDamageTempDamageRate;
        public float tempMovementSpeed;
        public float tempCooldownSpeed;
    }

    [System.Serializable]
    public struct ApplyTempStatFlag : IComponentData, IEnableableComponent
    {

    }
}
