using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class BootStrapPhaseAuthoring : EnableStructAuthorizer<BootStrapPhase>
    {

    }

    public class BootStrapPhaseBaker : EnableStructBaker<BootStrapPhaseAuthoring, BootStrapPhase>
    {
    }

    [System.Serializable]
    public struct BootStrapPhase : IComponentData, IEnableableComponent
    {
        public CameraMovement targetCameraMovement;
    }
}
