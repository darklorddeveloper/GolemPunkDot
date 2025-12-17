using Unity.Entities;
using UnityEngine;
namespace DarkLordGame
{
    [System.Serializable]
    public struct IntroPhase : IComponentData
    {
        public CameraMovement targetCameraMovement;
    }
}