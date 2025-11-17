using Unity.Entities;

namespace DarkLordGame
{
    [System.Serializable]
    public struct MovementSpeed : IComponentData
    {
        public float value;
        public float multiplier;
    }
}
