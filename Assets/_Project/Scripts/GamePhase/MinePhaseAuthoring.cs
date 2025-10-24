using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class MinePhaseAuthoring : EnableStructAuthorizer<MinePhase>
    {
        
    }

    public class MinePhaseBaker : EnableStructBaker<MinePhaseAuthoring, MinePhase>
    {
    }
    [System.Serializable]
    public struct MinePhase: IComponentData, IEnableableComponent
    {
        public CameraMovement targetCameraMovement;
    }
}
