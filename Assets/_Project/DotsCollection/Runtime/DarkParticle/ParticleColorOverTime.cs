using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace DarkLordGame
{

    public struct ParticleColorOverTime : IComponentData
    {
        public BlobAssetReference<Float4CurveBlob> data;
    }

    [MaterialProperty("_Color")]
    public struct ParticleColor : IComponentData
    {
        public float4 Value;
    }
}
