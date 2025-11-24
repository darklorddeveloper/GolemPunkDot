using Unity.Entities;
using UnityEngine;
namespace DarkLordGame
{

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
