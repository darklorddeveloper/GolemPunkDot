using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace DarkLordGame
{

    public class ParticleColorOverTimeAuthoring : MonoBehaviour
    {
        public bool looped;
        public Color testColor = Color.white;
        [GradientUsage(true)] public Gradient gradient = new();
        public class Baker : Baker<ParticleColorOverTimeAuthoring>
        {
            public override void Bake(ParticleColorOverTimeAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.Dynamic);
                if (authoring.gradient == null || (authoring.gradient.alphaKeyCount <= 0 && authoring.gradient.colorKeyCount <= 0))
                {
                    return;
                }
                var blob = Float4CurveUtility.CreateBlob(authoring.gradient, authoring.looped);
                var col = new float4(authoring.testColor.r, authoring.testColor.g, authoring.testColor.b, authoring.testColor.a);
                AddComponent(e, new ParticleColorOverTime
                {
                    Value = col,
                    data = blob
                });
            }
        }
    }
}