using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public struct TopdownCharacterMovement : IComponentData
    {
        public float movementSpeed;
        public float currentSpeed;
        public float accereration; //max by default
        public bool canTurnOnlyWhileMoving;
        public float turnSpeed; //max by default for best
        public bool useRaycast;
        public float size;
        public float castOffset;
        public uint layerBit;
        public uint collideWithLayerBit;
        public bool isHittedObstacle;
        public float3 hittedPoint;
        public float3 hittedNormal;
        public Entity lastHitEntity;
        public float lastMovedDistance;
    
    }
}
