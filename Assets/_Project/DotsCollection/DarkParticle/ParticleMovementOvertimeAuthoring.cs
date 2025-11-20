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
        public bool referenceStartDirection = true;
        public float3 direction;
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
                    direction = authoring.direction
                });
                if (authoring.referenceStartDirection)
                {
                    AddComponent(e, new ParticleStartDirection());
                }
            }
        }
    }

    public struct ParticleStartDirection : IComponentData, IEnableableComponent
    {

    }

    public struct ParticleMovementOvertime : IComponentData
    {
        public float maxDistance;
        public float3 position;
        public float3 direction;
        public BlobAssetReference<FloatCurveBlob> data;
    }
}
