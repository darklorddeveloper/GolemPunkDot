using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DarkLordGame
{
    public partial struct FlowFieldAgentJob : IJobEntity
    {
        [ReadOnly] public NativeArray<FlowFieldLayer> layers;
        [ReadOnly] public NativeArray<float3> directions;
        public void Execute(ref LocalTransform transform, ref FlowFieldAgent agent)
        {
            var layer = layers[agent.layer];
            var worldPos = transform.Position;
            var origin = layer.origin;
            float px = worldPos.x - origin.x;
            float pz = worldPos.z - origin.z;
            float qf = (math.sqrt(3f) / 3f * px - 1f / 3f * pz) / layer.cellSize;
            float rf = (2f / 3f * pz) / layer.cellSize;
            float xf = qf;
            float zf = rf;
            float yf = -xf - zf;

            // Cube rounding
            int rx = (int)math.round(xf);
            int ry = (int)math.round(yf);
            int rz = (int)math.round(zf);

            float dx = math.abs(rx - xf);
            float dy = math.abs(ry - yf);
            float dz = math.abs(rz - zf);

            if (dx > dy && dx > dz)
                rx = -ry - rz;
            else if (dy > dz)
                ry = -rx - rz;
            else
                rz = -rx - ry;

            var index = math.clamp(rx, 0, layer.x) + layer.x * math.clamp(rz, 0, layer.y);
            agent.movement = directions[index + layer.layerIndexOffset];
            agent.lastTargetPoint = layer.targetPoint;
            switch (layer.targetType)
            {
                case FlowFieldTargetType.Point:
                    agent.lastDistanceToTarget = math.distance(worldPos, layer.targetPoint);
                    break;
                case FlowFieldTargetType.XAxis:
                    agent.lastDistanceToTarget = math.abs(worldPos.x - layer.targetPoint.x);
                    break;
                case FlowFieldTargetType.YAxis:
                    agent.lastDistanceToTarget = math.abs(worldPos.y - layer.targetPoint.y);
                    break;
                case FlowFieldTargetType.ZAxis:
                    agent.lastDistanceToTarget = math.abs(worldPos.z - layer.targetPoint.z);
                    break;
            }
        }
    }
}
