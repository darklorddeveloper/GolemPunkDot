using Unity.Entities;
using Unity.Mathematics;

namespace DarkLordGame
{
    [System.Serializable]
    public struct FlowFieldAgent : IComponentData
    {
        public int layer;
        public float3 movement;
        public float3 lastTargetPoint;
        public float lastDistanceToTarget;
    }
}
