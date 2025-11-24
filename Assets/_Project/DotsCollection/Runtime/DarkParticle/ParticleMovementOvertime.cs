using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
  

    public struct ParticleStartPosition : IComponentData, IEnableableComponent
    {
    }

    public struct ParticleMovementOvertime : IComponentData
    {
        public float maxDistance;
        public bool isAdditiveMovement;
        public float3 startPosition;
        public float3 relativeDirection;
        public float3 movementDirection;
        public BlobAssetReference<FloatCurveBlob> data;
    }
}
