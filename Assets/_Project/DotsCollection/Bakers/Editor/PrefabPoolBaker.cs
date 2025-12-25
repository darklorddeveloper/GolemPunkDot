#if UNITY_EDITOR
using UnityEngine;
using Unity.Entities;
using UnityEditor;
namespace DarkLordGame
{
    public class PrefabPoolBaker : Baker<PrefabPoolAuthoring>
    {
        public override void Bake(PrefabPoolAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            int count = authoring.prefabs.Count;

            DynamicBuffer<PrefabPool> buff = AddBuffer<PrefabPool>(entity);
            for (int i = 0; i < count; i++)
            {
                buff.Add(new PrefabPool
                {
                    hash = new Unity.Entities.Hash128(
                        AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(authoring.prefabs[i]))
                        ),
                    prefab = GetEntity(authoring.prefabs[i], TransformUsageFlags.Dynamic)
                });
            }

            var orderRegistration = AddBuffer<OrderedPrefabPool>(entity);
            count = authoring.orderKeeping.Count;
            for (int i = 0; i < count; i++)
            {
                orderRegistration.Add(new OrderedPrefabPool
                {
                    prefab = GetEntity(authoring.orderKeeping[i], TransformUsageFlags.Dynamic)
                });
            }
            AddComponent(entity, new SetupPool());
            AddComponentObject(entity, new PoolMap());
        }
    }
}

#endif