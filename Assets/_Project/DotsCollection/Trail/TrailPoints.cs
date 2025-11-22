using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace DarkLordGame
{
    public struct SetupTrail : IComponentData, IEnableableComponent
    {

    }

    public struct TrailPoints : IBufferElementData
    {
        public float3 position;
        public quaternion rotation;
        public float timeCount;
    }


    [MaterialProperty("_P1")]
    public struct TrailP1 : IComponentData
    {
        public float3 Value;
    }

    [MaterialProperty("_P2")]
    public struct TrailP2 : IComponentData
    {
        public float3 Value;
    }

    [MaterialProperty("_P3")]
    public struct TrailP3 : IComponentData
    {
        public float3 Value;
    }

    [MaterialProperty("_P4")]
    public struct TrailP4 : IComponentData
    {
        public float3 Value;
    }

    [MaterialProperty("_P1Right")]
    public struct TrailP1Right : IComponentData
    {
        public float3 Value;
    }

    [MaterialProperty("_P2Right")]
    public struct TrailP2Right : IComponentData
    {
        public float3 Value;
    }

    [MaterialProperty("_P3Right")]
    public struct TrailP3Right : IComponentData
    {
        public float3 Value;
    }

    [MaterialProperty("_P4Right")]
    public struct TrailP4Right : IComponentData
    {
        public float3 Value;
    }
}
