using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace DarkLordGame
{
    public struct SetupTrail : IComponentData, IEnableableComponent
    {

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
}
