using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace DarkLordGame
{

    [MaterialProperty("_Color")]
    public struct ParticleColorOverTime : IComponentData
    {
        public float4 Value;
        public BlobAssetReference<Float4CurveBlob> data;
    }
}
