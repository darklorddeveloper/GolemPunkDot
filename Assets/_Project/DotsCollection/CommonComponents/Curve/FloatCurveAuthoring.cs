using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Unity.Entities.Serialization; 

namespace DarkLordGame
{
    public class FloatCurveAuthoring : MonoBehaviour
    {
        public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public class Baker : Baker<FloatCurveAuthoring>
        {
            public override void Bake(FloatCurveAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.Dynamic);
                var blobRef = FloatCurveUtility.CreateFloatCurveBlobReference(authoring.curve);
                AddComponent(e, new FloatCurve
                {
                    data = blobRef,
                });
            }
        }
    }
}
