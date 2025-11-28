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
}
