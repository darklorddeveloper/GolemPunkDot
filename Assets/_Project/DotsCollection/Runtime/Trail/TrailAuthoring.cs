using Unity.Entities;
using Unity.Mathematics;
namespace DarkLordGame
{

    [System.Serializable]
    public struct Trail : IComponentData
    {
        public int maxSegments;
        public int currentHeadSequmentIndex;//calculate start rate and pass through shader
        public float minDistance;
        public int maxSubDivision;//for far away distance
        public float maxSubDivisionCurve;//0.5 is good
        public float stayTime;
        public float lifeTimePerPeriod;
        public float3 lastPosition;
        public quaternion lastRotation;
        public float3 lastForward;
    }

    public struct TrailBones : IBufferElementData
    {
        public Entity entity;
        public float fixedSize;
    }


}