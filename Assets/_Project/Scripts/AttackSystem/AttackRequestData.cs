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
        public int extraSplit;
        public float splitAngle;

        public int loopCast;//minimal 1
        public int loopCasted;//minimal 1
        public float loopCastInterval;
        public float loopTimeCount;
        public float3 position;
        public quaternion rotation;
    }
}
