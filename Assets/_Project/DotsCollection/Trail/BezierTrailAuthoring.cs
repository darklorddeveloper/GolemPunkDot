using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace DarkLordGame
{
    public class BezierTrailAuthoring : MonoBehaviour
    {

    }

    public struct BezierTrail : IComponentData
    {
        public float timeCount;
        public float updatePointTime;
        
    }
}
