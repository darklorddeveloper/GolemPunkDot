using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public struct TopdownCharacterMovement : IComponentData
    {
        public bool canTurnOnlyWhileMoving; //settingdata
        public bool useRaycast;//settingdata
        public bool isHittedObstacle;
        public float turnSpeed; //settingdata
        public float size;//settingdata
        public float castOffset; //settingdata
        public float lastMovedDistance;
        public uint layerBit;//settingdata
        public uint collideWithLayerBit;//settingdata
        public float3 hittedPoint; //pending to remove might not needed
        public float3 hittedNormal;
        public Entity lastHitEntity; // might not needed
    
    }
}
