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
        //attack stat
        public float attackMultipler;
        public float aoeRange;
        public float damageRate;
        public float criticalChanceRate; //default 1
        public AttackProperty attackProperty;
        public int propertyValue;

        //attack itself stats
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
