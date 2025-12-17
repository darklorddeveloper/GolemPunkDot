using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
    //for AI just change to spawner and attack spawner is simply set
    [System.Serializable]
    public struct AttackRequestData : IComponentData, IEnableableComponent
    {
        [Header("Use for complex unit and powerful unit")]
        public Entity attacker;
        public Entity prefab;//entity with attack
        [Header("hit")]
        public uint belongToLayer;
        public uint hitLayer;
        //attack stat
        [Header("attack stats")]
        public float bonusDamage;
        public float attackDamageMultipler;
        public float aoeRange;
        public float aoeDamageRate;//bigger need to do with Unit attack system
        public bool useLimitedAngle;
        public float limitedDot;
        public float extraCritDamage; //default 1
        public float extracriticalChance; //default 1
        public AttackProperty attackProperty;
        public int propertyValue;

        //attack itself stats
        [Header("attack request stats")]//setting data
        public int extraSplit;
        public float splitAngle;
        public int loopCast;//minimal 1
        public int loopCasted;//minimal 1
        public float loopCastInterval;
        public float loopTimeCount;
        public int extraBounce;

        //attack movement stat
        [Header("Movement aspect")]//setting data
        public float extraLifeTime;
        public int bounce;
        public int chainAttack;

        [Header("Impact")]
        public float riftPower;//setting data
        public float pushPower;
        public bool canDealDeathImpact;
    }

    [System.Serializable]
    public struct AttackRequestTransform : IComponentData
    {
        public float forwardOffset;
        public float3 position;
        public quaternion rotation;
    }
}
