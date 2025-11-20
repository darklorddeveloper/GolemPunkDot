using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    public partial struct SetupBezierTrailJob : IJobEntity
    {
        public void Execute(ref BezierTrail bezierTrail, in LocalToWorld transform, EnabledRefRW<SetupTrail> trailSetup)
        {
            var pos = transform.Position;
            bezierTrail.currentP1 = pos;
            bezierTrail.currentP2 = pos;
            bezierTrail.currentP3 = pos;
            bezierTrail.targetP1 = pos;
            bezierTrail.targetP2 = pos;
            trailSetup.ValueRW = false;
        }
    }

    public partial struct BezierTrailJob : IJobEntity
    {
        public float deltaTime;
        public void Execute(ref BezierTrail trail, in LocalToWorld transform, ref TrailP1 p1, ref TrailP2 p2, ref TrailP3 p3)
        {
            trail.timeCount += deltaTime;
            if (trail.timeCount >= trail.updatePointsInterval)
            {
                trail.timeCount -= trail.updatePointsInterval;
                trail.currentP1 = trail.targetP1;
                trail.targetP1 = trail.targetP2;
                trail.currentP2 = trail.targetP2;
                trail.targetP2 = trail.currentP3;
            }
            trail.currentP3 = transform.Position;

            float weight = trail.timeCount / trail.updatePointsInterval;
            p1.Value = math.lerp(trail.currentP1, trail.targetP1, weight);
            p2.Value = math.lerp(trail.currentP2, trail.targetP2, weight);
            p3.Value = trail.currentP3;
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
