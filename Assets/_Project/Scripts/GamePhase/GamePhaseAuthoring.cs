using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class GamePhaseAuthoring : EnableStructAuthorizer<GamePhase>
    {
        
    }

    public class GamePhaseBaker : EnableStructBaker<GamePhaseAuthoring, GamePhase>
    {
    }
    [System.Serializable]
    public struct GamePhase : IComponentData, IEnableableComponent
    {
        public CameraMovement targetCameraMovement;
    }


}
