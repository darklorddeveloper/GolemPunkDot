using Unity.Entities;
using Unity.Rendering;

namespace DarkLordGame
{
    [MaterialProperty("_DamageTime")]
    public struct DamageTime : IComponentData
    {
        public float Value;
    }
}
