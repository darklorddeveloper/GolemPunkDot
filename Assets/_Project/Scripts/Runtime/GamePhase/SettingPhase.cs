using Unity.Entities;
namespace DarkLordGame
{
    [System.Serializable]
    public struct SettingPhase : IComponentData
    {
        public CameraMovement targetCameraMovement;
    }
}