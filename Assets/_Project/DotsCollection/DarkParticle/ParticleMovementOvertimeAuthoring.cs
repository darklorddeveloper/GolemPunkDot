using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
    public class ParticleMovementOvertimeAuthoring : MonoBehaviour
    {
        public bool looped;
        public float maxDistance = 1.0f;
        [Header("Keep time from 0 - 1")]
        public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public float3 relativeDirection = new float3(0, 0, 1);
        [Header("false = fixed direction true for more fancy random")]
        public bool shouldRealtimeUpdateDirection = false;
        public class Baker : Baker<ParticleMovementOvertimeAuthoring>
        {
            public override void Bake(ParticleMovementOvertimeAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.Dynamic);
                var blobRef = FloatCurveUtility.CreateFloatCurveBlobReference(authoring.curve, authoring.looped);
                // Let the baker dedupe and manage lifetime

                AddComponent(e, new ParticleMovementOvertime
                {
                    data = blobRef,
                    maxDistance = authoring.maxDistance,
                    relativeDirection = authoring.relativeDirection
                });
                AddComponent(e, new ParticleStartPosition { shouldKeepEnabled = authoring.shouldRealtimeUpdateDirection });
            }
        }
    }

    public struct ParticleStartPosition : IComponentData, IEnableableComponent
    {
        public bool shouldKeepEnabled;
        public bool updatedStartPos;
    }

    public struct ParticleMovementOvertime : IComponentData
    {
        public float maxDistance;
        public float3 startPosition;
        public float3 relativeDirection;
        public float3 movementDirection;
        public BlobAssetReference<FloatCurveBlob> data;
    }
}
