using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    public partial struct SetupBezierTrailJob : IJobEntity
    {
        public void Execute(Entity entity, ref BezierTrail bezierTrail, in LocalToWorld transform, EnabledRefRW<SetupTrail> trailSetup)
        {
            var pos = transform.Position;
            bezierTrail.currentP1 = pos;
            bezierTrail.currentP2 = pos;
            bezierTrail.currentP3 = pos;
            bezierTrail.targetP1 = pos;
            bezierTrail.targetP2 = pos;
            bezierTrail.targetP3 = pos;
            trailSetup.ValueRW = false;
        }
    }

    public partial struct BezierTrailJob : IJobEntity
    {
        float deltaTime;
        public void Execute()
        {

        }
    }

    public partial struct TrailSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {

        }
    }
}
