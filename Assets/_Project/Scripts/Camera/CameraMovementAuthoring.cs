using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
    public class CameraMovementAuthoring : StructAuthorizer<CameraMovement>
    {

    }

    public class CameraMovementBaker : StructBaker<CameraMovementAuthoring, CameraMovement>
    {
    }
    
    [System.Serializable]
    public struct CameraMovement : IComponentData
    {
        public float3 position;
        public float3 eulerRotation;
        public float movePeriod;

    }
}
