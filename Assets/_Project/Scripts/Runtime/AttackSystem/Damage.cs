using Unity.Entities;
using Unity.Mathematics;

namespace DarkLordGame
{
    public struct Damage : IComponentData, IEnableableComponent
    {
        public float damageToken;
        public float3 impactDirection;
        public float3 damagePosition; //needed this for pushback so change to push back
        public float3 damageSourcePosition;
    }


    public struct KnockBack : IComponentData
    {
        public float3 direction;
    }

    [System.Serializable]
    public struct TakeDamageAnimationPeriod : IComponentData
    {
        public float period;
    }
    public struct AoeDamage : IComponentData, IEnableableComponent
    {
        public float3 position;
        public float3 forward;
        public Attack attack;
    }
}
