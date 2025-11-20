using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class ParticleColorOverTimeAuthoring : MonoBehaviour
    {
        public bool looped;
        [GradientUsage(true)]public Gradient gradient;        
        public class Baker : Baker<ParticleColorOverTimeAuthoring>
        {
            public override void Bake(ParticleColorOverTimeAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.Dynamic);
                var blob = Float4CurveUtility.CreateBlob(authoring.gradient, authoring.looped);
                AddComponent(e, new ParticleColorOverTime
                {
                    data = blob
                });
            }
        }
    }

    public struct ParticleColorOverTime : IComponentData
    {
        public BlobAssetReference<Float4CurveBlob> data;
    }
}
