using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class GamePhaseAuthoring : EnableStructAuthorizer<GamePhase, GamePhaseIntro>
    {
        
    }

    public class GamePhaseBaker : EnableStructBaker<GamePhaseAuthoring, GamePhase, GamePhaseIntro>
    {
    }
    [System.Serializable]
    public struct GamePhase : IComponentData, IEnableableComponent
    {
        public CameraMovement targetCameraMovement;
    }

    [System.Serializable]
    public struct GamePhaseIntro : IComponentData, IEnableableComponent
    {
        
    }
}
