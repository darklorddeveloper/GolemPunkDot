using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public struct AttackRequestData : IComponentData, IEnableableComponent
    {
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
        public float extraCritDamage; //default 1
        public float extracriticalChance; //default 1
        public AttackProperty attackProperty;
        public int propertyValue;

        //attack itself stats
        [Header("attack request stats")]
        public int extraSplit;
        public float splitAngle;
        public int loopCast;//minimal 1
        public int loopCasted;//minimal 1
        public float loopCastInterval;
        public float loopTimeCount;
        public int extraBounce;
        public float3 position;
        public quaternion rotation;

        //attack movement stat
        [Header("Movement aspect")]
        public float extraLifeTime;
        public float bounce;
        public float chainAttack;
    }

    [System.Serializable]
    public struct AttackRequestAutoAssignAttacker : IComponentData, IEnableableComponent
    {

    }

    [System.Serializable]
    public struct AttackRequestAutoAssignPrefab : IComponentData, IEnableableComponent
    {
        public Entity prefab;
    }
}
