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
                AddComponent<TrailP1>(e);
                AddComponent<TrailP2>(e);
                AddComponent<TrailP3>(e);
                AddComponent<TrailP1Right>(e);
                AddComponent<TrailP2Right>(e);
                AddComponent<TrailP3Right>(e);

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
        public float3 currentP1;
        public float3 currentP2;
        public float3 currentP3;

        public float3 targetP1Right;
        public float3 targetP2Right;
        public float3 currentP1Right;
        public float3 currentP2Right;
        public float3 currentP3Right;
    }
}
