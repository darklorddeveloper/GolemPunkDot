using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct DeathUnitJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, Entity entity, in Death death)
        {
        }
    }

    public partial struct DeathSystem : ISystem
    {
    }
}
