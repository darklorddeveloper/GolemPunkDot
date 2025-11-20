using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace DarkLordGame
{
    public class BezierTrailAuthoring : MonoBehaviour
    {
        public float updatePointInterval = 0.45f;
        public class Baker : Baker<BezierTrailAuthoring>
        {
            public override void Bake(BezierTrailAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(e, new BezierTrail
                {
                    updatePointsInterval = authoring.updatePointInterval
                });
                AddComponent<SetupTrail>(e);
            }
        }
    }

    public struct BezierTrail : IComponentData
    {
        public float timeCount;
        public float updatePointsInterval;

        public float3 targetP1;
        public float3 targetP2;
        public float3 targetP3;
        public float3 currentP1;
        public float3 currentP2;
        public float3 currentP3;
    }
}
