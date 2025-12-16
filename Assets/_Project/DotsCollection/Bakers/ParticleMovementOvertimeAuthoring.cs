using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
    public class ParticleMovementOvertimeAuthoring : MonoBehaviour
    {
        public bool looped;
        [Header("Max distance is speed when additive movement")]
        public bool isAdditiveMovement;
        public float maxDistance = 1.0f;
        [Header("Keep time from 0 - 1")]
        public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public float3 relativeDirection = new float3(0, 0, 1);
        public class Baker : Baker<ParticleMovementOvertimeAuthoring>
        {
            public override void Bake(ParticleMovementOvertimeAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.Dynamic);
                var blobRef = FloatCurveUtility.CreateFloatCurveBlobReference(authoring.curve, authoring.looped);
                // Let the baker dedupe and manage lifetime
                AddBlobAsset(ref blobRef, out var _);
                AddComponent(e, new ParticleMovementOvertime
                {
                    data = blobRef,
                    maxDistance = authoring.maxDistance,
                    relativeDirection = authoring.relativeDirection,
                    isAdditiveMovement = authoring.isAdditiveMovement
                });
                AddComponent(e, new ParticleStartPosition { });
            }
        }
    }
}