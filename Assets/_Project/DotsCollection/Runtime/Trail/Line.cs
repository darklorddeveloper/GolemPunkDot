using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
namespace DarkLordGame
{
    [System.Serializable]
    public struct Line : IComponentData
    {
        public float3 startPosition;
        public float3 endPosition;
    }

    [System.Serializable]
    public struct DynamicLine : IComponentData
    {

    }

    public struct Setupline : IComponentData, IEnableableComponent
    {

    }
}
