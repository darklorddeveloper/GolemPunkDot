using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
    public struct SpawnerTransform : IComponentData, IEnableableComponent
    {
        public float3 position;
        public float3 rotation;
    }
}
