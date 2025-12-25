using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public partial struct FlowFieldCalculationSystem : ISystem
    {
        private NativeArray<FlowCell> cells;
        private NativeArray<FlowFieldLayer> layers;

        private bool isInitialized;

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
                    cells = new NativeArray<FlowCell>(totalNumbersOfCell, Allocator.Persistent);
                    for (int i = 0, length = layers.Length; i < length; i++)
                    {
                        var layer = layers[i];
                        for (int j = 0, num = layer.x * layer.y; j < num; j++)
                        {
                            var index = layer.layerIndexOffset + j;
                            cells[index] = new FlowCell
                            {
                                layer = i,

                            };
                        }
                    }
                    isInitialized = true;
                }
                return;
            }

            //first distance
            //make the dynamic cost
        }
    }
}
