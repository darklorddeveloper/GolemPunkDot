using Unity.Entities;
using Unity.Rendering;

namespace DarkLordGame
{

    
    [System.Serializable]
    [MaterialProperty("_StartTime")]
    public struct StartTime : IComponentData, IEnableableComponent
    {
        public float Value;
    }
}
