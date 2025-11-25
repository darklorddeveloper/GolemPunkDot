using Unity.Entities;
namespace DarkLordGame
{
    [System.Serializable]
    public struct SettingPhase : IComponentData, IEnableableComponent
    {
        public CameraMovement targetCameraMovement;
    }
}