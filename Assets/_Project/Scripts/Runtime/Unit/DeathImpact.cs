using Unity.Entities;
using Unity.Mathematics;

namespace DarkLordGame
{
    [System.Serializable]
    public struct DeathImpact : IComponentData
    {
        public float3 forcePoint;
        public float3 sourcePosition;
    }

    [System.Serializable]
    public struct DeathImpactMovement : IComponentData
    {
        public float velocityPerDistance; // 1.0f
        public float maxHeight;
        public float horizontalPower;
        public float gravitySpeed;
        public float dampingSpeed;
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
