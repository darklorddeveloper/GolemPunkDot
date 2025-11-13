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
        Freeze = 8,
        Knockback = 16,
    }

    [System.Serializable]//only set in runtime
    public struct Attack : IComponentData
    {
        //hit damage related
        public float damage;
        public float attackMultiplier;//
        public float aoeRange; //hit fx ----
        public float criticalChance;
        public float extraCritDamage;
        public AttackProperty attackProperty;
        public float propertyValue;

        //movement related need to think about this
        // public float lifeTime;
        // public float bounce;
        // public float chainAttack;
    }

    [System.Serializable]//attach in prefabs
    public struct HitEffect : IComponentData
    {
        public Entity entity;
    }
}
