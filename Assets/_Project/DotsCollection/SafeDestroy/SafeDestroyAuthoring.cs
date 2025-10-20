using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class SafeDestroyAuthoring : MonoBehaviour
    {
        public bool shouldDestroyFromStart;
        public float period;
        public class Baker : Baker<SafeDestroyAuthoring>
        {
            public override void Bake(SafeDestroyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SafeDestroyComponent { period = authoring.period });
                SetComponentEnabled<SafeDestroyComponent>(entity, authoring.shouldDestroyFromStart); ;
            }
        }
    }

    public struct SafeDestroyComponent : IComponentData, IEnableableComponent
    {
        public float period;
    }
}
