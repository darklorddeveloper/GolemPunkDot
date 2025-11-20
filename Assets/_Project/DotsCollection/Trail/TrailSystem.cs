using System.Diagnostics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    public partial struct SetupBezierTrailJob : IJobEntity
    {
        public void Execute(ref BezierTrail bezierTrail, in LocalToWorld transform, EnabledRefRW<SetupTrail> trailSetup,
        ref TrailP1 p1, ref TrailP2 p2, ref TrailP3 p3,
        ref TrailP1Right p1r, ref TrailP2Right p2r, ref TrailP3Right p3r)
        {
            var pos = transform.Position;
            var right = transform.Right;
            bezierTrail.currentP1 = pos;
            bezierTrail.currentP2 = pos;
            bezierTrail.currentP3 = pos;

            bezierTrail.targetP1 = pos;
            bezierTrail.targetP2 = pos;
            p1.Value = pos;
            p2.Value = pos;
            p3.Value = pos;
            p1r.Value = right;
            p2r.Value = right;
            p3r.Value = right;
            bezierTrail.currentP1Right = right;
            bezierTrail.currentP2Right = right;
            bezierTrail.currentP3Right = right;
            bezierTrail.targetP1Right = right;
            bezierTrail.targetP2Right = right;
            trailSetup.ValueRW = false;
        }
    }

    public partial struct BezierTrailJob : IJobEntity
    {
        public float deltaTime;
        public void Execute(ref BezierTrail trail,
        in LocalToWorld transform,
        ref TrailP1 p1, ref TrailP2 p2, ref TrailP3 p3,
        ref TrailP1Right p1r, ref TrailP2Right p2r, ref TrailP3Right p3r)
        {
            trail.timeCount += deltaTime;
            if (trail.timeCount >= trail.updatePointsInterval)
            {
                trail.timeCount -= trail.updatePointsInterval;
                trail.currentP1 = trail.targetP1;
                trail.targetP1 = trail.targetP2;
                trail.currentP2 = trail.targetP2;
                trail.targetP2 = trail.currentP3;

                trail.currentP1Right = trail.targetP1Right;
                trail.targetP1Right = trail.targetP2Right;
                trail.currentP2Right = trail.targetP2Right;
                trail.targetP2Right = trail.currentP3Right;
            }
            trail.currentP3 = transform.Position;
            trail.currentP3Right = transform.Right;

            float weight = trail.timeCount / trail.updatePointsInterval;
            p1.Value = math.lerp(trail.currentP1, trail.targetP1, weight);
            p2.Value = math.lerp(trail.currentP2, trail.targetP2, weight);
            p3.Value = trail.currentP3;

            p1r.Value = math.lerp(trail.currentP1Right, trail.targetP1Right, weight);
            p2r.Value = math.lerp(trail.currentP2Right, trail.targetP2Right, weight);
            p3r.Value = trail.currentP3Right;
        }
    }

    public partial struct TrailSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var job1 = new SetupBezierTrailJob();
            job1.ScheduleParallel();
            float delta = SystemAPI.Time.DeltaTime;
            var job2 = new BezierTrailJob { deltaTime = delta };
            job2.ScheduleParallel();
        }
    }
}
