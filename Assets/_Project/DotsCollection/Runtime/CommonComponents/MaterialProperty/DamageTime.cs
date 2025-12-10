using Unity.Entities;
using Unity.Rendering;

namespace DarkLordGame
{
    [MaterialProperty("_DamageTime")]
    [System.Serializable]
    public struct DamageTime : IComponentData, IEnableableComponent
    {
        public float Value;
    }
}
