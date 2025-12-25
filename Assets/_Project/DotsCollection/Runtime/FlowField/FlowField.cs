using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkLordGame
{
    public enum FlowFieldTargetType
    {
        Point,//float3
        XAxis,
        YAxis,
        ZAxis,
    }
//todo for later project --- flowfield dynamic add

    [System.Serializable]
    public struct FlowFieldLayer : IBufferElementData
    {
        //active state
        public int layerIndexOffset; //can be setup in baker x*y
        public int x, y;//grid size
        public float cellSize;      //go with horizontal size 2x vertical root 3 
        public float3 origin;
        public FlowFieldTargetType targetType;
        public float3 targetPoint;
    }

    [System.Serializable]
    public struct DynamicFlowFieldOrigin : IComponentData
    {
        public int layer;
    }
    
    [System.Serializable]
    public struct DynamicFlowFieldTarget : IComponentData
    {
        public int layer;
        public FloatField targetType;
    }

    public struct FlowCell
    {
        public int layer;
        public int x, y;
        public float3 localPosition;
    }
}
