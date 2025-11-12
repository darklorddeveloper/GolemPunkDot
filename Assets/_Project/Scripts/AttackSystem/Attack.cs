using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public enum AttackProperty
    {
        None = 0,
        Poison = 1,
        Burn = 2,
        Stun = 4,
        Knockback = 8,
    }

    [System.Serializable]//only set in runtime
    public struct Attack : IComponentData
    {
        //hit target
        public float damage;
        public float aoeRange; //hit fx ----
        //AOEID
        public float criticalChance;
        public float extraCritDamage;
        public AttackProperty attackProperty;
        public float propertyValue;
        public float lifeTime;
        public float bounce;
        public float chainAttack;
    }

    [System.Serializable]//attach in prefabs
    public struct HitEffect : IComponentData
    {

    }
}
