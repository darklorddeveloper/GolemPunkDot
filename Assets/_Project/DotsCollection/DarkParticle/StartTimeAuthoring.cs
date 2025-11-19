using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace DarkLordGame
{

    public class StartTimeAuthoring : EnableStructAuthorizer<StartTime>
    {

    }

    public class StartTimeBaker : EnableStructBaker<StartTimeAuthoring, StartTime>
    {

    }
    
    [System.Serializable]
    [MaterialProperty("_StartTime")]
    public struct StartTime : IComponentData, IEnableableComponent
    {
        public float Value;
    }
}
