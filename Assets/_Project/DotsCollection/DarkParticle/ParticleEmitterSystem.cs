using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEditorInternal;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct ParticleEmitterJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public Random random;
        public float deltaTime;
        public void Execute([ChunkIndexInQuery] int chunk, Entity entity, ref DynamicBuffer<ParticleEmitter> emitters, in LocalTransform localTransform, in ParticleEmitterDestroyWhenFinished whenFinished)
        {
            random.state += (uint)chunk;
            bool isAnyEnabled = false;
            for (int i = 0, length = emitters.Length; i < length; i++)
            {
                var emitter = emitters[i];
                if (emitter.isEnabled == false)
                {
                    continue;
                }
                isAnyEnabled = true;
                emitter.delayTimeCount += deltaTime;
                if (emitter.delayTimeCount < emitter.delayed)
                {
                    emitters[i] = emitter;
                    continue;
                }

                emitter.timeCount += deltaTime;
                if (emitter.timeCount < emitter.interval)
                {
                    emitters[i] = emitter;
                    continue;
                }
                for (int j = 0, numbers = emitter.emmitNumbersPerInterval; j < numbers; j++)
                {
                    var e = ecb.Instantiate(chunk, emitter.prefab);
                    var loc = localTransform;
                    switch (emitter.shapeType)
                    {
                        case EmitShape.None:
                            ecb.SetComponent(chunk, e, loc);
                            break;
                        case EmitShape.Sphere:
                            loc.Position += random.NextFloat3(new float3(-emitter.shapeSize), new float3(emitter.shapeSize));
                            ecb.SetComponent(chunk, e, loc);

                            break;
                    }
                }
                if (emitter.isLooped || emitter.loopCount > 0)
                {
                    emitter.timeCount -= emitter.interval;
                    emitter.loopCount--;
                }
                else
                {
                    emitter.isEnabled = false;
                }
                emitters[i] = emitter;
            }//for
            if (whenFinished.destroy && isAnyEnabled == false)
            {
                ecb.DestroyEntity(chunk, entity);
            }
        }
    }

    [BurstCompile]
    public partial struct ParticleEmitterSystem : ISystem
    {
        private uint seed;
        private Random rand;

        public void OnCreate(ref SystemState state)
        {
            seed = 0;
            rand = Random.CreateFromIndex(seed);
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            float deltaTime = SystemAPI.Time.DeltaTime;

            seed++;
            rand = Random.CreateFromIndex(seed);

            var job = new ParticleEmitterJob
            {
                ecb = ecb.AsParallelWriter(),
                deltaTime = deltaTime,
                random = rand,
            };
            var handle = job.ScheduleParallel(state.Dependency);
            handle.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
