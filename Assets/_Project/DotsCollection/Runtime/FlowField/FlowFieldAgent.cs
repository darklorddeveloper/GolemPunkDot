using Unity.Entities;
using Unity.Mathematics;

namespace DarkLordGame
{
    [System.Serializable]
    public struct FlowFieldAgent : IComponentData
    {
        public int layer;
        public float lastDistanceToTarget;
        public float3 movement;
        public float3 lastTargetPoint;
    }
}
