using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
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
        public bool useParentRotation;
    }


    public struct ParticleEmitterDestroyWhenFinished : IComponentData
    {
        public bool destroy;
    }
}
