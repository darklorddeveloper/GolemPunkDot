using Unity.Entities;

namespace DarkLordGame
{

    public class ParticleAuthoring : EnableStructAuthorizer<Particle>
    {

    }

    public class ParticleBaker : EnableStructBaker<ParticleAuthoring, Particle>
    {
    }

    [System.Serializable]
    public struct Particle : IComponentData, IEnableableComponent
    {
        public bool isInfiniteLoop;
        public int loopCount;
        public float lifeTime;
        public float timeCount;
        public float currentRate;
    }

}
