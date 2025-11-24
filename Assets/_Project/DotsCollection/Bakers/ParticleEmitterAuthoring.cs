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
}