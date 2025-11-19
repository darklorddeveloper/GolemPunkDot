using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
    [RequireComponent(typeof(SafeDestroyAuthoring))]
    public class ParticleEmitterAuthoring : MonoBehaviour
    {
        public bool shouldDestroyWhenFinished = false;
        public List<ParticleEmitterBakerData> emitters = new List<ParticleEmitterBakerData>();
        public class Baker : Baker<ParticleEmitterAuthoring>
        {
            public override void Bake(ParticleEmitterAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(e, new Spawner { spawner = Entity.Null });
                var buff = AddBuffer<ParticleEmitter>(e);
                for (int i = 0, length = authoring.emitters.Count; i < length; i++)
                {
                    var prefab = GetEntity(authoring.emitters[i].prefab, TransformUsageFlags.Dynamic);
                    var emitter = authoring.emitters[i].particleEmitter;
                    emitter.prefab = prefab;
                    emitter.isEnabled = authoring.emitters[i].enableFromTheBegin;
                    if (authoring.emitters[i].useFirstBurst)
                    {
                        emitter.timeCount = emitter.interval;
                    }
                    buff.Add(emitter);
                }
                AddComponent(e, new ParticleEmitterDestroyWhenFinished { destroy = authoring.shouldDestroyWhenFinished });

            }
        }
    }

    public enum EmitShape
    {
        None,
        Sphere,
    }

    [System.Serializable]
    public class ParticleEmitterBakerData
    {
        [Header("----- Warning: prefab must be particle or have spawner -----")]
        public GameObject prefab;
        public bool enableFromTheBegin = true;
        public bool useFirstBurst = false;
        public ParticleEmitter particleEmitter;
    }

    [System.Serializable]
    public struct ParticleEmitter : IBufferElementData
    {
        public Entity prefab;
        public bool isEnabled;
        public bool isLooped;
        public int loopCount;
        public float delayed;
        public float delayTimeCount;
        public float interval;
        public float timeCount;
        public int emmitNumbersPerInterval;
        public EmitShape shapeType;
        public float shapeSize;
        public float3 shapeSize3D;
    }


    public struct ParticleEmitterDestroyWhenFinished : IComponentData
    {
        public bool destroy;
    }
}
