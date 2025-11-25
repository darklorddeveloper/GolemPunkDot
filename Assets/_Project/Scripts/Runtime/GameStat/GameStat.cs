using Unity.Entities;

namespace DarkLordGame
{
    [System.Serializable]
    public struct GameStat : IComponentData
    {
        public Entity player;
    }
}
