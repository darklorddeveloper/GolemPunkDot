using UnityEngine;
using Unity.Entities;
namespace DarkLordGame
{
    public class SafeDestroyAuthoring : MonoBehaviour
    {
        public bool shouldDestroyFromStart;
        public float period;
        public bool destroyChild = true;
        public class Baker : Baker<SafeDestroyAuthoring>
        {
            public override void Bake(SafeDestroyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SafeDestroyComponent { period = authoring.period });
                SetComponentEnabled<SafeDestroyComponent>(entity, authoring.shouldDestroyFromStart);
                AddComponent(entity, new DestroyImmediate { destroyChild = authoring.destroyChild });
                SetComponentEnabled<DestroyImmediate>(entity, false);

            }
        }
    }

}
