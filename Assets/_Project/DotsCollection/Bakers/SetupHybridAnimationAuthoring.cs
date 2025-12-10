using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
namespace DarkLordGame
{
    public class SetupHybridAnimationAuthoring : ClassAuthorizer<SetupHybridAnimation>
    {
        public bool hasLocomotion;
        public float locomotionLerpSpeed = 10.0f;
    }

    public class SetupHybridAnimationBaker : ClassBaker<SetupHybridAnimationAuthoring, SetupHybridAnimation>
    {
        public override void Bake(SetupHybridAnimationAuthoring authoring)
        {
            base.Bake(authoring);
            var e = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<TransformSync>(e);
            AddComponent<HybridAnimation>(e);
            AddComponent<PlayHybridAnimation>(e);
            if (authoring.hasLocomotion)
            {
                AddComponent(e, new HybridLocomotion { lerpSpeed = authoring.locomotionLerpSpeed });
            }
        }
    }
}
