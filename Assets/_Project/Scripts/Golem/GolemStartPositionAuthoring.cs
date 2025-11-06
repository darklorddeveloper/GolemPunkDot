using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{

    public class GolemStartPositionAuthoring : StructAuthorizer<GolemStartPosition>
    {

    }

    public class GolemStartPositionBaker : StructBaker<GolemStartPositionAuthoring, GolemStartPosition>
    {

    }

    [System.Serializable]
    public struct GolemStartPosition : IComponentData
    {
        public float3 position;
        public float3 targetPosition;
    }
}
