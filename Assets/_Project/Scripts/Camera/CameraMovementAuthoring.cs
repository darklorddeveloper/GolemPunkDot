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

    public struct CameraMovement : IComponentData
    {
        public float3 position;
        public float3 offsetToTarget;
        public float3 targetPosition;
        public quaternion rotation;
        public float movementTimeCount;
        public float movePeriod;
        
    }
}
