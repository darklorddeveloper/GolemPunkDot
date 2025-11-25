using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public struct GamePhase : IComponentData, IEnableableComponent
    {
        public CameraMovement targetCameraMovement;
    }
}