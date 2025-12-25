using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public struct ResultPhase : IComponentData
    {
        public CameraMovement targetCameraMovement;
    }
}
