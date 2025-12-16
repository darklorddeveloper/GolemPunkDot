using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [InternalBufferCapacity(0)]
    public struct InstanceAnimation : IBufferElementData
    {
        public Entity target;
    }

    [System.Serializable]
    public struct PlayInstanceAnimation : IComponentData, IEnableableComponent
    {
    }

    [System.Serializable]
    public struct CurrentInstanceAnimationIndex : IComponentData
    {
        public int index;
    }
}
