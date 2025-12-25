using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct FlowFieldUpdateOriginJob : IJobEntity
    {
        [NativeDisableParallelForRestriction] public NativeArray<FlowFieldLayer> layers;
        public void Execute(in LocalTransform localTransform, in DynamicFlowFieldOrigin origin)
        {
            var layer = layers[origin.layer];
            layer.origin = localTransform.Position;
        }
    }

    [BurstCompile]
    public partial struct FlowFieldUpdateTargetJob : IJobEntity
    {
        [NativeDisableParallelForRestriction] public NativeArray<FlowFieldLayer> layers;
        public void Execute(in LocalTransform localTransform, in DynamicFlowFieldTarget target)
        {
            var layer = layers[target.layer];
            layer.targetPoint = localTransform.Position;
        }
    }

    [BurstCompile]
    public struct FlowFieldDistanceCostJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<FlowCell> cells;
        [ReadOnly] public NativeArray<FlowFieldLayer> layers;
        public NativeArray<float> costs;
        public void Execute(int i)
        {
            var cell = cells[i];
            var layer = layers[cell.layer];
            var pos = cell.localPosition + layer.origin;
            switch (layer.targetType)
            {
                case FlowFieldTargetType.Point:
                    costs[i] = math.distancesq(pos, layer.targetPoint); // do static items in the future
                    break;
                case FlowFieldTargetType.XAxis:
                    costs[i] = math.abs(pos.x - layer.targetPoint.x);
                    break;
                case FlowFieldTargetType.YAxis:
                    costs[i] = math.abs(pos.y - layer.targetPoint.y);
                    break;
                case FlowFieldTargetType.ZAxis:
                    costs[i] = math.abs(pos.z - layer.targetPoint.z);
                    break;
            }
        }
    }

    [BurstCompile]
    public partial struct FlowFieldDirectionJob : IJobParallelFor
    {
        public NativeArray<float3> directions;//make direction struct when there is another need
        [ReadOnly] public NativeArray<FlowCell> cells;
        [ReadOnly] public NativeArray<float> costs;
        [ReadOnly] public NativeArray<FlowFieldLayer> layers;
        [ReadOnly] public NativeArray<int2> borderIndice;

        public void Execute(int index)
        {
            var cell = cells[index];
            var cost = costs[index];
            var layer = layers[cell.layer];
            float minCost = math.INFINITY;
            float3 direction = float3.zero;
            var pos = cell.localPosition;
            for (int i = 0, length = borderIndice.Length; i < length; i++)
            {
                var x = cell.x + borderIndice[i].x;
                var y = cell.y + borderIndice[i].y;
                if (x < 0 || x >= layer.x || y < 0 || y >= layer.y)
                {
                    continue;//out of bound
                }
                int borderIndex = layer.layerIndexOffset + y * layer.x + layer.x;
                var border = cells[borderIndex];
                var d = costs[borderIndex];
                if (d < minCost)
                {
                    direction = border.localPosition - pos;
                    minCost = d;
                }
            }
            directions[index] = direction;
            // layer.x
        }
    }

    [BurstCompile]
    public partial struct FlowFieldCalculationSystem : ISystem
    {
        private NativeArray<FlowCell> cells;
        private NativeArray<float> costs;//make cost struct when there is another need
        private NativeArray<float3> directions;//make direction struct when there is another need
        private NativeArray<FlowFieldLayer> layers;
        private NativeArray<int2> borderIndice;
        private bool isInitialized;

        private static float root3 = math.sqrt(3.0f);

        public void OnCreate(ref SystemState state)
        {
            borderIndice = new NativeArray<int2>(6, Allocator.Persistent);
            borderIndice[0] = new int2(+1, 0); // E
            borderIndice[1] = new int2(+1, -1); // NE
            borderIndice[2] = new int2(0, -1); // NW
            borderIndice[3] = new int2(-1, 0); // W
            borderIndice[4] = new int2(-1, +1); // SW
            borderIndice[5] = new int2(0, +1); // SE
        }

        public void OnUpdate(ref SystemState state)
        {
            //init
            if (isInitialized == false)
            {
                if (SystemAPI.TryGetSingletonBuffer<FlowFieldLayer>(out var buffer))
                {
                    int totalNumbersOfCell = 0;
                    layers = buffer.ToNativeArray(Allocator.Persistent);
                    for (int i = 0, length = layers.Length; i < length; i++)
                    {
                        var layer = layers[i];
                        layer.layerIndexOffset = totalNumbersOfCell;
                        totalNumbersOfCell += layer.x * layer.y;
                        layers[i] = layer;
                    }
                    cells = new NativeArray<FlowCell>(totalNumbersOfCell, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
                    costs = new NativeArray<float>(totalNumbersOfCell, Allocator.Persistent);
                    directions = new NativeArray<float3>(totalNumbersOfCell, Allocator.Persistent);
                    for (int i = 0, length = layers.Length; i < length; i++)
                    {
                        var layer = layers[i];
                        for (int j = 0, num = layer.x * layer.y; j < num; j++)
                        {
                            var index = layer.layerIndexOffset + j;
                            var x = j % layer.x;
                            var y = j / layer.x;
                            float posX = layer.cellSize * root3 * (x + y / 2);
                            float posZ = layer.cellSize * 1.5f * y;
                            cells[index] = new FlowCell
                            {
                                layer = i,
                                x = x,
                                y = y,
                                localPosition = new float3(posX, 0, posZ)
                            };
                        }
                    }
                    isInitialized = true;
                }
                return;
            }

            //update origin
            var updateOrigin = new FlowFieldUpdateOriginJob
            {
                layers = layers
            };
            state.Dependency = updateOrigin.Schedule(state.Dependency);

            //update target
            var updatetarget = new FlowFieldUpdateTargetJob
            {
                layers = layers
            };
            state.Dependency = updatetarget.Schedule(state.Dependency);

            //first distance cost
            var distanceJob = new FlowFieldDistanceCostJob
            {
                cells = cells,
                layers = layers,
                costs = costs
            };
            state.Dependency = distanceJob.Schedule(cells.Length, 64, state.Dependency);

            //make the dynamic cost


            //direction
            var directionJob = new FlowFieldDirectionJob
            {
                borderIndice = borderIndice,
                costs = costs,
                cells = cells,
                layers = layers,
                directions = directions
            };
            state.Dependency = directionJob.Schedule(cells.Length, 64, state.Dependency);
            //agent job
            //resolver system after this
        }
    }
}
