using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class SettingPhaseAuthoring : EnableStructAuthorizer<SettingPhase>
    {

    }

    public class SettingPhaseBaker : EnableStructBaker<SettingPhaseAuthoring, SettingPhase>
    {
    }

    [System.Serializable]
    public struct SettingPhase : IComponentData, IEnableableComponent
    {
        public CameraMovement targetCameraMovement;
    }
}
