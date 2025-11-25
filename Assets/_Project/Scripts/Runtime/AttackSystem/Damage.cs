using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
    public struct Damage : IComponentData
    {
        public Entity target;
        public Attack attack;
    }

    public struct AoeDamage : IComponentData
    {
        public float3 position;
        public Attack attack;
    }
}
