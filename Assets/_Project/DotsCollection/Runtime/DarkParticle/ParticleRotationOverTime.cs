using Unity.Entities;
using Unity.Mathematics;

namespace DarkLordGame
{
    public struct ParticleRotationOverTime : IComponentData
    {
        public float maxAngles;
        public float3 axis;
        public quaternion startRotation;
        public BlobAssetReference<FloatCurveBlob> data;
    }

    public struct ParticleStartRotation : IComponentData, IEnableableComponent
    {
    }
}
