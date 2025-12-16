using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public struct TopdownCharacterMovement : IComponentData
    {
        public bool canTurnOnlyWhileMoving; //setting related
        public bool useRaycast;//setting related
        public bool isHittedObstacle;
        public float turnSpeed; //setting related
        public float size;//setting related
        public float castOffset; //setting related
        public float lastMovedDistance;
        public uint layerBit;//setting related
        public uint collideWithLayerBit;//setting related
        public float3 hittedPoint;
        public float3 hittedNormal;
        public Entity lastHitEntity;
    
    }
}
