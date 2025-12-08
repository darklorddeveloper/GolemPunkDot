using Unity.Entities;
using Unity.Mathematics;

namespace DarkLordGame
{
    [System.Serializable]
    public struct DeathImpact : IComponentData
    {
        public float3 velocity;
    }

    [System.Serializable]
    public struct DeathImpactMovement : IComponentData
    {
        public float velocityRate;//1.0f 
        public float maxHeight;
        public float horizontalPower;
        public float gravitySpeed;
        public float dampingSpeed;
    }

    [System.Serializable]
    public struct DealDeathImpactDamage : IComponentData
    {

    }

    [System.Serializable]
    public struct DeathImpactDamage : IComponentData
    {
        public Attack attack;
    }
}
