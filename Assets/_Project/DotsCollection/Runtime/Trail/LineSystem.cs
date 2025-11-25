using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct SetupLineJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;

        public void Execute([ChunkIndexInQuery] int chunk, EnabledRefRW<Setupline> setup, in Line line, in LocalToWorld transform, DynamicBuffer<TrailBones> bones)
        {
            float3 translation = transform.Position;       // parent world pos
            quaternion rotation = math.inverse(transform.Rotation);
            float3 diff = line.endPosition - line.startPosition;
            quaternion rot = quaternion.LookRotation(math.normalize(diff), new float3(0, 1, 0));//world
            var localRot = math.mul(rotation, rot);//local
            float rate = 1.0f / (bones.Length - 1);
            for (int i = 0, length = bones.Length; i < length; i++)
            {
                var pos = math.lerp(line.startPosition, line.endPosition, rate * i);
                float3 localPos = math.mul(rotation, pos - translation);
                ecb.SetComponent(chunk, bones[i].entity, new LocalTransform { Position = localPos, Rotation = localRot, Scale = bones[i].fixedSize });
            }
            setup.ValueRW = false;
        }
    }

    [BurstCompile]
    public partial struct UpdateLineJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;

        public void Execute([ChunkIndexInQuery] int chunk, in Line line, in DynamicLine dynamicLine, in LocalToWorld transform, DynamicBuffer<TrailBones> bones)
        {
            float3 translation = transform.Position;       // parent world pos
            quaternion rotation = math.inverse(transform.Rotation);
            float3 diff = line.endPosition - line.startPosition;
            quaternion rot = quaternion.LookRotation(math.normalize(diff), new float3(0, 1, 0));//world
            var localRot = math.mul(rotation, rot);//local
            float rate = 1.0f / (bones.Length - 1);
            for (int i = 0, length = bones.Length; i < length; i++)
            {
                var pos = math.lerp(line.startPosition, line.endPosition, rate * i);
                float3 localPos = math.mul(rotation, pos - translation);
                ecb.SetComponent(chunk, bones[i].entity, new LocalTransform { Position = localPos, Rotation = localRot, Scale = bones[i].fixedSize });
            }
        }
    }

    [BurstCompile]
    public partial struct LineSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
            var trailUpdate = new SetupLineJob
            {
                ecb = ecb.AsParallelWriter(),
            };
            var handle = trailUpdate.ScheduleParallel(state.Dependency);
            handle.Complete();

            var dynamicLineJob = new UpdateLineJob
            {
                ecb = ecb.AsParallelWriter()
            };
            handle = dynamicLineJob.ScheduleParallel(state.Dependency);
            handle.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
