using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public struct TopdownCharacterMovement : IComponentData
    {
        public float movementSpeed;
        public float currentSpeed;
        public float accereration; //max by default
        public float turnSpeed; //max by default for best
        public float size;
        public float castOffset;
        public uint layerBit;
        public uint collideWithLayerBit;
    
    }
}
