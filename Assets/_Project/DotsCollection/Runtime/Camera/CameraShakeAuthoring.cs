using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class CameraShakeAuthoring : EnableStructAuthorizer<CameraShake>
    {
        
    }
    
    public class CameraShakeBaker : EnableStructBaker<CameraShakeAuthoring, CameraShake>
    {
    }

    [System.Serializable]
    public struct CameraShake : IComponentData, IEnableableComponent
    {
        public float period;
        public float power;
    }
}
