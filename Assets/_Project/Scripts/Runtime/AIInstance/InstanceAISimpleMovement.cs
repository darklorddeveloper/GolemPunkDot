using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{

    [System.Serializable]
    public struct InstanceAISimpleMovement : IComponentData
    {
        public bool isAvoiding;
        public bool isPreviouslyAvoiding;
        public float approachDistanceSquare;

        [Header("Min Max Attack Range")]
        public float approachMinDistance;
        public float approachMaxDistance;//attack range

        public half3 avoidingDirection;
        public float avoidanceTimeCount;
        public float avoidancePeriod;
    }

    // public struct InstanceAISimpleMovementSettingBlob
    // {
    //     [Header("Min Max Attack Range")]
    //     public float approachMinDistance;
    //     public float approachMaxDistance;//attack range
    //     public float avoidancePeriod;
    // }
}
