using Unity.Entities;
using Unity.Mathematics;

namespace DarkLordGame
{
    [System.Serializable]
    public struct DeathImpact : IComponentData
    {
        public float3 velocityDirection;
        public float3 sourcePosition;
    }

    public struct InitDeathImpact : IComponentData, IEnableableComponent
    {
    }

    [System.Serializable]
    public struct DeathImpactMovement : IComponentData, IEnableableComponent
    {
        public float velocityLostPerDistance; // 1.0f
        public float maxHeight;
        public float gravity;
        public float damping;
    }

    [System.Serializable]
    public struct DealDeathImpactDamage : IComponentData, IEnableableComponent
    {
        public float radius;
    }

    [System.Serializable]
    public struct DeathImpactDamage : IComponentData
    {
        public Attack attack;
    }
}
