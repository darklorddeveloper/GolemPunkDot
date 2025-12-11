using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{

    [System.Serializable]
    public struct InstanceAIMovement : IComponentData
    {
        public float approachDistanceSquare;

        [Header("Min Max Attack Range")]
        public float approachMinDistance;
        public float approachMaxDistance;//attack range
    }
}
