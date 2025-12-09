using Unity.Entities;
using Unity.Rendering;

namespace DarkLordGame
{
    
    [MaterialProperty("_StartTime")]
    public struct StartTime : IComponentData, IEnableableComponent
    {
        public float Value;
    }
}
