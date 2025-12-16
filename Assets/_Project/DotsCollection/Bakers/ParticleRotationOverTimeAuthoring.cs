using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
namespace DarkLordGame
{
    public class ParticleRotationOverTimeAuthoring : MonoBehaviour
    {
        public bool looped = false;
        public float maxAngle = 1.0f;
        public float3 axis = new float3(0, 1, 0);
        [Header("Keep time from 0 - 1")]
        public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public class Baker : Baker<ParticleRotationOverTimeAuthoring>
        {
            public override void Bake(ParticleRotationOverTimeAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.Dynamic);
                var blobRef = FloatCurveUtility.CreateFloatCurveBlobReference(authoring.curve, authoring.looped);
                AddBlobAsset(ref blobRef, out var _);
                // Let the baker dedupe and manage lifetime
                AddComponent(e, new ParticleRotationOverTime
                {
                    data = blobRef,
                    axis = authoring.axis,
                    maxAngles = math.radians(authoring.maxAngle)
                });

                AddComponent(e, new ParticleStartRotation());
            }
        }
    }
}