using Unity.Entities;
using Unity.Mathematics;

namespace DarkLordGame
{
    [System.Serializable]
    public struct WallPosition : IComponentData
    {
        public float3 position;
    }
}
