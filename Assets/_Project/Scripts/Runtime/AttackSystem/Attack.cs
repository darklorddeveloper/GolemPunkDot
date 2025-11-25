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
        [Header("Hit")]
        public uint belongToLayer;
        public uint hitLayer;
        [Header("stat")]
        public float damage;
        public float aoeRange; //hit fx ----
        public float aoeDamage;
        public float criticalChance;
        public float criticalDamage;
        public AttackProperty attackProperty;
        public float propertyValue;
        //movement related need to think about this
        // public float lifeTime;
        public int bounce;
        public int chainAttack;
    }

    [System.Serializable]//attach in prefabs
    public struct HitEffect : IComponentData
    {
        //must contain spawner component
        public Entity entity;
    }
}
