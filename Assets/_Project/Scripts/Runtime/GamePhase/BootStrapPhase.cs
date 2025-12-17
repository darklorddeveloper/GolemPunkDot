using Unity.Entities;
namespace DarkLordGame
{
   [System.Serializable]
    public struct BootStrapPhase : IComponentData
    {
        public CameraMovement targetCameraMovement;
    }
}
