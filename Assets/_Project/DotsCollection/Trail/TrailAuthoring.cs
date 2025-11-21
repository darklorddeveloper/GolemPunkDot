using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEditor;
using UnityEngine;
namespace DarkLordGame
{
    public class TrailAuthoring : MonoBehaviour
    {
        public Trail trail;
        public List<GameObject> bones = new();
        public AnimationCurve trailCurve = AnimationCurve.Linear(0, 1, 1, 0);
        public class Baker : Baker<TrailAuthoring>
        {
            public override void Bake(TrailAuthoring authoring)
            {
                var trail = authoring.trail;
                if (authoring.bones == null) return;
                trail.maxSegments = authoring.bones.Count;
                if (trail.maxSegments <= 1) return;

                var e = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(e, trail);
                var points = AddBuffer<TrailPoints>(e);

                var bones = AddBuffer<TrailBones>(e);
                float rate = authoring.bones.Count <= 1 ? 1 : (authoring.bones.Count - 1);
                for (int i = 0, length = authoring.bones.Count; i < length; i++)
                {
                    if (authoring.bones[i] == null) continue;
                    var bone = GetEntity(authoring.bones[i], TransformUsageFlags.Dynamic);
                    float scale = authoring.trailCurve.Evaluate((float)i / rate);
                    bones.Add(new TrailBones { entity = bone, fixedSize = scale });
                }

                points.Resize(trail.maxSegments, Unity.Collections.NativeArrayOptions.ClearMemory);
                // forwards.Resize(trail.maxSegments, Unity.Collections.NativeArrayOptions.ClearMemory);
                AddComponent<SetupTrail>(e);
            }
        }

    }
    [System.Serializable]
    public struct Trail : IComponentData
    {
        public int maxSegments;
        public int currentHeadSequmentIndex;//calculate start rate and pass through shader
        public float minDistance;
        public int maxSubDivision;//for far away distance
        public float maxSubDivisionCurve;//0.5 is good
        public float3 lastPosition;
        public quaternion lastRotation;
        public float3 lastForward;
    }

    public struct TrailBones : IBufferElementData
    {
        public Entity entity;
        public float fixedSize;
    }


}