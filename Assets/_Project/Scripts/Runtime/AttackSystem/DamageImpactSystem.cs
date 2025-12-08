using Unity.Burst;
using Unity.Entities;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct ImpactJob : IJobEntity
    {
        public void Execute()
        {
            
        }
    }

    [BurstCompile]
    public partial struct DamageImpactSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {

        }
    }
}
