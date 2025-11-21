using System.Diagnostics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    public partial struct SetupTrailJob : IJobEntity
    {
        public void Execute(ref Trail trail, DynamicBuffer<TrailPoints> points, EnabledRefRW<SetupTrail> setupTrail, in LocalToWorld transform)
        {
            var pos = transform.Position;

            var rot = transform.Rotation;
            trail.lastPosition = pos;
            trail.lastRotation = rot;
            trail.lastForward = transform.Forward;
            for (int i = 0, length = trail.maxSegments; i < length; i++)
            {
                points[i] = new TrailPoints { position = float3.zero, rotation = quaternion.identity };
            }
            setupTrail.ValueRW = false;
        }
    }

    public partial struct TrailJob : IJobEntity
    {
        public void Execute(ref Trail trail, LocalToWorld transform, DynamicBuffer<TrailPoints> points)
        {
            var pos = transform.Position;
            var forward = transform.Forward;
            var rot = transform.Rotation;
            var diff = pos - trail.lastPosition;
            var length = math.length(diff);
            if (length < trail.minDistance)
            {
                return;
            }
            // points[trail.currentHeadSequmentIndex] = new TrailPoints { position = pos, rotation = rot };
            // trail.currentHeadSequmentIndex++;
            // trail.currentHeadSequmentIndex %= trail.maxSegments;
            int segmentToAdd = math.clamp((int)math.floor(length / trail.minDistance), 1, trail.maxSubDivision);
            var point = points[(trail.currentHeadSequmentIndex + trail.maxSegments - 2) % trail.maxSegments];
            float weightPerSegment = 1.0f / segmentToAdd;
            var lastForward = trail.lastPosition - point.position;
            var middlePoint = math.dot(diff, lastForward) * trail.maxSubDivisionCurve * lastForward + pos;

            for (int i = 0; i < segmentToAdd; i++)
            {
                var weight = (i + 1) * weightPerSegment;
                var invert = 1 - weight;
                var p = invert * invert * trail.lastPosition + 2 * invert * weight * middlePoint +
                weight * weight * pos;
                var r = math.slerp(trail.lastRotation, rot, weight);
                points[trail.currentHeadSequmentIndex] = new TrailPoints { position = p, rotation = r };
                trail.currentHeadSequmentIndex++;
                trail.currentHeadSequmentIndex %= trail.maxSegments;

            }
            trail.lastPosition = pos;
            trail.lastRotation = rot;
            trail.lastForward = forward;
        }
    }

    public partial struct TrailsUpdateJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, in Trail trail, in LocalToWorld ltw, DynamicBuffer<TrailBones> bones, DynamicBuffer<TrailPoints> points)
        {
            float3 translation = ltw.Position;       // parent world pos
            quaternion rotation = ltw.Rotation;
            // var previousPoint = ltw.Position;
            for (int i = 0, length = bones.Length; i < length; i++)
            {
                int index = (trail.currentHeadSequmentIndex + trail.maxSegments - i - 1) % trail.maxSegments;
                var targetPoint = points[index];
                float3 localPos = math.mul(math.inverse(rotation), targetPoint.position - translation);
                index = (trail.currentHeadSequmentIndex + trail.maxSegments * 2 - i - 2) % trail.maxSegments;
                targetPoint = points[index];
                float3 localPos2 = math.mul(math.inverse(rotation), targetPoint.position - translation);
                // var localRot = math.mul(math.inverse(ltw.Rotation), targetPoint.rotation);

                var rot = quaternion.LookRotation(localPos - localPos2, new float3(0, 1, 0));
                ecb.SetComponent(chunk, bones[i].entity, new LocalTransform
                {
                    Position = localPos,
                    Rotation = rot,
                    Scale = bones[i].fixedSize
                });


                // UnityEngine.Debug.DrawLine(targetPoint.position, previousPoint, Color.red);
                // previousPoint = targetPoint.position;
            }

        }
    }

    // public partial struct SetupBezierTrailJob : IJobEntity
    // {
    //     public void Execute(ref BezierTrail bezierTrail, in LocalToWorld transform, EnabledRefRW<SetupTrail> trailSetup,
    //     ref TrailP1 p1, ref TrailP2 p2, ref TrailP3 p3,
    //     ref TrailP1Right p1r, ref TrailP2Right p2r, ref TrailP3Right p3r)
    //     {
    //         var pos = transform.Position;
    //         var right = transform.Right;
    //         bezierTrail.currentP1 = pos;
    //         bezierTrail.currentP2 = pos;
    //         bezierTrail.currentP3 = pos;

    //         bezierTrail.targetP1 = pos;
    //         bezierTrail.targetP2 = pos;
    //         p1.Value = pos;
    //         p2.Value = pos;
    //         p3.Value = pos;
    //         p1r.Value = right;
    //         p2r.Value = right;
    //         p3r.Value = right;
    //         bezierTrail.currentP1Right = right;
    //         bezierTrail.currentP2Right = right;
    //         bezierTrail.currentP3Right = right;
    //         bezierTrail.targetP1Right = right;
    //         bezierTrail.targetP2Right = right;
    //         trailSetup.ValueRW = false;
    //     }
    // }

    // public partial struct BezierTrailJob : IJobEntity
    // {
    //     public float deltaTime;
    //     public void Execute(ref BezierTrail trail,
    //     in LocalToWorld transform,
    //     ref TrailP1 p1, ref TrailP2 p2, ref TrailP3 p3,
    //     ref TrailP1Right p1r, ref TrailP2Right p2r, ref TrailP3Right p3r)
    //     {
    //         trail.timeCount += deltaTime;
    //         if (trail.timeCount >= trail.updatePointsInterval)
    //         {
    //             trail.timeCount -= trail.updatePointsInterval;
    //             trail.currentP1 = trail.targetP1;
    //             trail.targetP1 = trail.targetP2;
    //             trail.currentP2 = trail.targetP2;
    //             trail.targetP2 = trail.currentP3;

    //             trail.currentP1Right = trail.targetP1Right;
    //             trail.targetP1Right = trail.targetP2Right;
    //             trail.currentP2Right = trail.targetP2Right;
    //             trail.targetP2Right = trail.currentP3Right;
    //         }
    //         trail.currentP3 = transform.Position;
    //         trail.currentP3Right = transform.Right;

    //         float weight = trail.timeCount / trail.updatePointsInterval;
    //         p1.Value = math.lerp(trail.currentP1, trail.targetP1, weight);
    //         p2.Value = math.lerp(trail.currentP2, trail.targetP2, weight);
    //         p3.Value = trail.currentP3;

    //         p1r.Value = math.lerp(trail.currentP1Right, trail.targetP1Right, weight);
    //         p2r.Value = math.lerp(trail.currentP2Right, trail.targetP2Right, weight);
    //         p3r.Value = trail.currentP3Right;
    //     }
    // }

    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial struct TrailSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var setupJob = new SetupTrailJob();
            setupJob.ScheduleParallel();
            var trailJob = new TrailJob();
            trailJob.ScheduleParallel();
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
            var trailUpdate = new TrailsUpdateJob
            {
                ecb = ecb.AsParallelWriter()
            };
            var handle = trailUpdate.ScheduleParallel(state.Dependency);
            handle.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
