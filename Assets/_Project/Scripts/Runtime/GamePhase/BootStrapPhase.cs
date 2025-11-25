using Unity.Entities;
namespace DarkLordGame
{
   [System.Serializable]
    public struct BootStrapPhase : IComponentData, IEnableableComponent
    {
        public CameraMovement targetCameraMovement;
    }
}
