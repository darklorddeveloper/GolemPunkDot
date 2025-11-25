using Unity.Entities;
using Unity.Mathematics;

namespace DarkLordGame
{
    [System.Serializable]
    public struct GolemStartPosition : IComponentData
    {
        public float3 position;
        public float3 targetPosition;
    }
}