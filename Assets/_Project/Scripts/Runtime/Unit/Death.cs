using Unity.Entities;

namespace DarkLordGame
{
    [System.Serializable]
    public struct Death : IComponentData, IEnableableComponent
    {
        
    }

    [System.Serializable]
    public struct DeathEffect : IComponentData
    {
        public Entity prefab;
        
    }
}
