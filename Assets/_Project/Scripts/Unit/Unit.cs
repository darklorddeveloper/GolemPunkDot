using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public struct Unit : IComponentData
    {
        public float HP;
        public float MaxHP;
        public float tempMaxHP;
        public float shield;//always temp
        public float attack;
        public float tempAttack;
        public float criticalChance;//50%
        public float tempCriticalChance;
        public float criitcalDamage;//1.5f
        public float tempCriticalDamage;
        public float bonusAoeDamage;
        public float bonusAoeRange;

        public float movementSpeed;
        public float tempMovementSpeed;
        public float cooldownSpeed;
        public float tempCooldownSpeed;


    }
    [System.Serializable]
    public struct TempStatCounter : IComponentData, IEnableableComponent
    {
        public float tempAttackCounter;
        public float tempCritChanceCounter;
        public float tempCritDamageCounter;
        public float tempMovementSpeedCounter;
        public float tempCooldownSpeedCounter;
    }
}
