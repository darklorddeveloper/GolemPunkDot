using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public struct InstanceAnimationDelayedPlay : IComponentData, IEnableableComponent
    {
        public int targetIndex;
        public float period;
        public float timeCount;
    }
}
