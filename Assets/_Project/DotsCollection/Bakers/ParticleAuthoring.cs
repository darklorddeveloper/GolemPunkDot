using Unity.Entities;
using UnityEngine;
namespace DarkLordGame
{
    public class ParticleAuthoring : EnableStructAuthorizer<Particle>
    {
        public bool shouldDestroyFromStart = true;
        public float fixedLifeTimeWhenUsedInfiniteLoop = 5;
        public bool doNotDestroy;
        public bool destroyChildParticle = true;
    }

    public class ParticleBaker : EnableStructBaker<ParticleAuthoring, Particle>
    {
        public override void Bake(ParticleAuthoring authoring)
        {
            base.Bake(authoring);
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Spawner>(entity);

            if (authoring.doNotDestroy == false)
            {
                float period = authoring.data1.isInfiniteLoop ? authoring.fixedLifeTimeWhenUsedInfiniteLoop : (authoring.data1.lifeTime * Mathf.Max(authoring.data1.loopCount, 1));
                AddComponent(entity, new SafeDestroyComponent { period = period });
                SetComponentEnabled<SafeDestroyComponent>(entity, authoring.shouldDestroyFromStart);
                AddComponent(entity, new DestroyImmediate { destroyChild = authoring.destroyChildParticle });
                SetComponentEnabled<DestroyImmediate>(entity, false);
            }
        }
    }
}