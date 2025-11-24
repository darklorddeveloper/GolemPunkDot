using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct SpawnerTransformJob : IJobEntity
    {
        public ComponentLookup<LocalTransform> spawnerLookup;
        public void Execute(ref SpawnerTransform spawnerTransform, EnabledRefRW<SpawnerTransform> spawnTransformEnable, in Spawner spawner)
        {
            if (spawnerLookup.TryGetComponent(spawner.spawner, out var localTransform))
            {
                spawnerTransform.position = localTransform.Position;
                spawnerTransform.rotation = localTransform.Rotation;
                return;
            }
            spawnTransformEnable.ValueRW = false;
        }
    }
    [BurstCompile]
    public partial struct SpawnerTransformSystem : ISystem
    {
        public ComponentLookup<LocalTransform> componentLookup;
        public void OnCreate(ref SystemState state)
        {
            componentLookup = state.GetComponentLookup<LocalTransform>();
        }

        public void OnUpdate(ref SystemState state)
        {
            componentLookup.Update(ref state);
            var job = new SpawnerTransformJob
            {
                spawnerLookup = componentLookup,
            };
            job.ScheduleParallel();
        }
    }
}
