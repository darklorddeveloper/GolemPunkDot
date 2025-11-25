using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
 
    [System.Serializable]
    public struct CameraMovement : IComponentData
    {
        public float3 position;
        public float3 eulerRotation;
        public float movePeriod;

    }
}
