using Unity.Collections;
using Unity.Entities;

using UnityEngine;

namespace DarkLordGame
{
    public class SizeCurveAuthoring : MonoBehaviour
    {
        public bool looped;
        public float maxSize = 1.0f;
        [Header("Keep time from 0 - 1")]
        public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public bool referenceStartSize = true;
        public class Baker : Baker<SizeCurveAuthoring>
        {
            public override void Bake(SizeCurveAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.Dynamic);
                var blobRef = FloatCurveUtility.CreateFloatCurveBlobReference(authoring.curve, authoring.looped);
                // Let the baker dedupe and manage lifetime
                AddComponent(e, new ParticleSizeOverTime
                {
                    data = blobRef,
                    maxSize = authoring.maxSize
                });
                if(authoring.referenceStartSize)
                {
                    AddComponent(e, new ParticleStartSize());
                }
            }
        }
    }

    public struct ParticleSizeOverTime : IComponentData
    {
        public float maxSize;
        public BlobAssetReference<FloatCurveBlob> data;
    }

    public struct ParticleStartSize : IComponentData, IEnableableComponent
    {
    }
}
