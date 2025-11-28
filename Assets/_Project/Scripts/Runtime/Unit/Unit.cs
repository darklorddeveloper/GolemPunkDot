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
        public float bonusMaxHP;
        public float shield;//always temp
        public float tempShield;
        public float attack;
        public float tempAttack;
        public float tempAttackMultiplier;
        public float criticalChance;//50%
        public float tempCriticalChance;
        public float criitcalDamage;//1.5f
        public float tempCriticalDamage;
        public float bonusAoeDamageRate;
        public float bonusAoeDamageTempDamageRate;
        public float bonusAoeRange;

        public float movementSpeed;
        public float tempMovementSpeed;
        public float cooldownSpeed;
        public float tempCooldownSpeed;
    }

    [System.Serializable]
    public struct ChangeMovementSpeed : IComponentData, IEnableableComponent
    {

    }
}
