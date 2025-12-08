using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
    public struct Damage : IComponentData, IEnableableComponent
    {
        public Attack attack;
        public float3 damagePosition;
        public float3 damageSourcePosition;
    }

    public struct AoeDamage : IComponentData, IEnableableComponent
    {
        public float3 position;
        public float3 forward;
        public Attack attack;
    }
}
