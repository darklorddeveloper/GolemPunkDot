using Unity.Entities;

namespace DarkLordGame
{
    [System.Serializable]
    public struct HybridLocomotion : IComponentData
    {
        public float lerpSpeed;
        public float x;
        public float y;
    }
}
