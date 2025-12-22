using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.UIElements;

namespace DarkLordGame
{
    public partial struct WaveProgressSystem : ISystem
    {
        private BufferLookup<WaveDataCounter> _counterLookup;
        private uint frameCount;
        public void OnCreate(ref SystemState state)
        {
            _counterLookup = state.GetBufferLookup<WaveDataCounter>(isReadOnly: false);
            state.RequireForUpdate<WaveData>();
            frameCount = 0;
        }

        public void OnUpdate(ref SystemState state)
        {
            _counterLookup.Update(ref state);
            float deltaTime = SystemAPI.Time.DeltaTime;
            frameCount++;
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
            foreach (var (wavDatas, unit, transform, e) in SystemAPI.Query<DynamicBuffer<WaveData>, Unit, LocalTransform>().WithEntityAccess().WithAll<WaveDataCounter>())
            {
                var counters = _counterLookup[e];

                for (int i = 0, length = wavDatas.Length; i < length; i++)
                {
                    var waveData = wavDatas[i];
                    var counter = counters[i];
                    if (waveData.isInfinite == false && counter.spawned >= waveData.numbersPerSpawn)
                    {
                        continue;
                    }
                    float percentage = unit.HP / unit.MaxHP;
                    if (percentage <= waveData.activateAtPercentage)
                    {
                        counter.timeCount += deltaTime;
                        if (counter.timeCount > waveData.interval)
                        {
                            counter.timeCount = 0;
                            var entitiesArray = new NativeArray<Entity>(waveData.numbersPerSpawn, Allocator.Temp);
                            ecb.Instantiate(waveData.prefab, entitiesArray);
                            var forward = transform.Forward() * waveData.spawnOffset;
                            var rot = quaternion.LookRotationSafe(forward, new float3(0, 1, 0));
                            var pos = forward + transform.Position;

                            if (waveData.spawnEffect != Entity.Null)
                            {
                                var spawnFX = new NativeArray<Entity>(waveData.numbersPerSpawn, Allocator.Temp);
                                for (int j = 0, num = entitiesArray.Length; i < num; j++)
                                {
                                    var rand = Random.CreateFromIndex(frameCount);
                                    var angle = rand.NextFloat(-waveData.splitAngle, waveData.splitAngle);
                                    var extraRot = quaternion.Euler(0, angle, 0);
                                    var rotation = math.mul(extraRot, rot);
                                    var p = math.mul(extraRot, forward) + pos;
                                    ecb.SetComponent(entitiesArray[i], new LocalTransform { Position = p, Rotation = rotation, Scale = 1.0f });
                                    ecb.SetComponent(spawnFX[i], new LocalTransform { Position = p, Rotation = rotation, Scale = 1.0f });
                                }
                                spawnFX.Dispose();
                            }
                            else
                            {
                                for (int j = 0, num = entitiesArray.Length; i < num; j++)
                                {
                                    var rand = Random.CreateFromIndex(frameCount);
                                    var angle = rand.NextFloat(-waveData.splitAngle, waveData.splitAngle);
                                    var extraRot = quaternion.Euler(0, angle, 0);
                                    var rotation = math.mul(extraRot, rot);
                                    var p = math.mul(extraRot, forward) + pos;
                                    ecb.SetComponent(entitiesArray[i], new LocalTransform { Position = p, Rotation = rotation, Scale = 1.0f });
                                }
                            }

                            if (waveData.isInfinite == false)
                            {
                                counter.spawned += waveData.numbersPerSpawn;
                            }
                            entitiesArray.Dispose();

                        }
                        counters[i] = counter;
                        continue;
                    }

                    break;
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
