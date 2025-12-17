using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct SpawnAttackJob : IJobEntity
    {
        public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk,
        EnabledRefRW<SpawnAttackRequest> enableRequest,
        in SpawnAttackRequestSetting requestSetting,
        ref SpawnAttackCounter counter,
        in LocalTransform localTransform)
        {
            ref var setting = ref requestSetting.setting.Value;
            counter.timeCount += deltaTime;
            if (counter.loopCount >= 1 && counter.timeCount < setting.loopCastInterval)
            {
                return;
            }
            counter.timeCount = 0;
            counter.loopCount++;

            var pos = localTransform.Position + localTransform.Forward() * setting.forwardOffset;
            var rot = localTransform.Rotation;
            if (setting.split > 0)
            {
                int split = setting.split + 1;
                float startAngle = -setting.splitAngle * (split - 1.0f) / 2.0f;
                UnityEngine.Debug.Log("here spawn multiple");

                //instantiate                
                NativeArray<Entity> entities = new NativeArray<Entity>(split, Allocator.TempJob);
                ecb.Instantiate(chunk, requestSetting.prefab, entities);
                for (int i = 0; i <= split; i++)
                {
                    //set position and split
                    var extraRot = quaternion.Euler(0, 0, startAngle + i * setting.splitAngle);
                    var targetRot = math.mul(rot, extraRot);
                    ecb.SetComponent(chunk, entities[i], new LocalTransform { Position = pos, Rotation = targetRot, Scale = 1.0f });
                }
                entities.Dispose();
            }
            else
            {
                var e = ecb.Instantiate(chunk, requestSetting.prefab);
                ecb.SetComponent(chunk, e, new LocalTransform { Position = pos, Rotation = rot, Scale = 1.0f });
            }

            if (counter.loopCount >= setting.loopCast)
            {
                counter.loopCount = 0;
                enableRequest.ValueRW = false;
            }
        }
    }

    [BurstCompile]
    [UpdateAfter(typeof(InstanceAIStateSystem))]
    public partial struct SpawnAttackSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = new EntityCommandBuffer( Allocator.TempJob);
            var job = new SpawnAttackJob
            {
                deltaTime = deltaTime,
                ecb = ecb.AsParallelWriter()
            };
            var handle = job.ScheduleParallel(state.Dependency);
            handle.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
