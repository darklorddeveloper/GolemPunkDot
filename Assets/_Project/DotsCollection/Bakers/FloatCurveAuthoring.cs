using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Unity.Entities.Serialization; 

namespace DarkLordGame
{
    public class FloatCurveAuthoring : MonoBehaviour
    {
        public bool looped;
        public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public class Baker : Baker<FloatCurveAuthoring>
        {
            public override void Bake(FloatCurveAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.Dynamic);
                var blobRef = FloatCurveUtility.CreateFloatCurveBlobReference(authoring.curve, authoring.looped);
                
                AddComponent(e, new FloatCurve
                {
                    
                    data = blobRef,
                });
            }
        }
    }
}
