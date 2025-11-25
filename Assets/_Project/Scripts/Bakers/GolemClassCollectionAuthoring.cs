using UnityEngine;
using Unity.Entities;

namespace DarkLordGame
{
    public class GolemClassCollectionAuthoring : MonoBehaviour
    {
        public GameObject[] prefabs;

        public class Baker : Baker<GolemClassCollectionAuthoring>
        {
            public override void Bake(GolemClassCollectionAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                var buffer = AddBuffer<GolemClassCollection>(entity);
                if (authoring.prefabs == null || authoring.prefabs.Length == 0)
                    return;

                buffer.EnsureCapacity(authoring.prefabs.Length);

                foreach (var go in authoring.prefabs)
                {
                    if (go == null) continue;

                    // Modern way: this both declares & bakes the referenced GameObject as an Entity prefab.
                    var prefabEntity = GetEntity(go, TransformUsageFlags.Dynamic);
                    buffer.Add(new GolemClassCollection { prefab = prefabEntity });
                }
            }
        }
    }

}
