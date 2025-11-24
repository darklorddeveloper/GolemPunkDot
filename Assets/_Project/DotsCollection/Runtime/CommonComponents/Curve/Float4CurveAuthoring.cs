using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class Float4CurveAuthoring : MonoBehaviour
    {
        public bool looped = true;
        public float period = 1.0f;
        [GradientUsage(true)] public Gradient gradient;

        public class Baker : Baker<Float4CurveAuthoring>
        {
            public override void Bake(Float4CurveAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.Dynamic);
                var blob = Float4CurveUtility.CreateBlob(authoring.gradient, authoring.looped);
                AddComponent(e, new Float4Curve
                {
                    data = blob,
                    period = authoring.period,
                });
            }
        }
    }
}
