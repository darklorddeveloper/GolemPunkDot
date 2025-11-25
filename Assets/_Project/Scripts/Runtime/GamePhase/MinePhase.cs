using Unity.Entities;
namespace DarkLordGame
{
    [System.Serializable]
    public struct MinePhase : IComponentData, IEnableableComponent
    {
        public CameraMovement targetCameraMovement;
    }
}