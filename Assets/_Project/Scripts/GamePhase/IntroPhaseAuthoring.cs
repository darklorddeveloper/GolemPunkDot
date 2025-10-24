using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class IntroPhaseAuthoring : EnableStructAuthorizer<IntroPhase>
    {
        
    }

    public class IntroPhaseBaker : EnableStructBaker<IntroPhaseAuthoring, IntroPhase>
    {
    }
    
    [System.Serializable]
    public struct IntroPhase : IComponentData, IEnableableComponent
    {
        public CameraMovement targetCameraMovement;
    }
}
