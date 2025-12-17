using Unity.Entities;
namespace DarkLordGame
{
    [System.Serializable]
    public struct MinePhase : IComponentData
    {
        public CameraMovement targetCameraMovement;
    }
}