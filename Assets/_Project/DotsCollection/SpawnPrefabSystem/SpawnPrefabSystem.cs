using Unity.Entities;
using UnityEngine;
using System.Collections.Generic;
using System;
using Hash128 = Unity.Entities.Hash128;
using Unity.Collections;
namespace DarkLordGame
{
    public partial class SetupPrefabSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<SetupPool>();
        }

        protected override void OnUpdate()
        {
            foreach (var (setupPool, prefabPool, map, entity) in SystemAPI.Query<SetupPool, DynamicBuffer<PrefabPool>, PoolMap>().WithEntityAccess())
            {
                for (int i = 0, length = prefabPool.Length; i < length; i++)
                {
                    map.prefabMaps = new Dictionary<Unity.Entities.Hash128, int>();
                    if (map.prefabMaps.ContainsKey(prefabPool[i].hash)) continue;
                    map.prefabMaps[prefabPool[i].hash] = i;
                }
                EntityManager.SetComponentEnabled<SetupPool>(entity, false);
            }
        }
    }

    [UpdateAfter(typeof(SetupPrefabSystem))]
    public partial class SpawnPrefabSystem : SystemBase
    {
        public static Func<string, Entity> SpawnPrefab;
        public static Action<string, NativeArray<Entity>> SpawnPrefabs;
        public static Func<int, Entity> SpawnPrefabWithID;
        public static Action<int, NativeArray<Entity>> SpawnPrefabsWithID;

        protected override void OnCreate()
        {
            base.OnCreate();
            SpawnPrefab = OnSpawnPrefab;
            SpawnPrefabs = OnSpawnPrefabs;
            SpawnPrefabWithID = OnSpawnPrefabWithID;
            SpawnPrefabsWithID = OnSpawnPrefabsWithID;
        }

        private Entity OnSpawnPrefab(string GUID)
        {
            Hash128 hash = new Hash128(GUID);
            if (SystemAPI.ManagedAPI.TryGetSingleton<PoolMap>(out var poolMap) == false)
            {
                return Entity.Null;

            }
            if (poolMap.prefabMaps.ContainsKey(hash) == false)
            {
                return Entity.Null;
            }

            DynamicBuffer<PrefabPool> buffer = SystemAPI.GetSingletonBuffer<PrefabPool>();
            int prefabindex = poolMap.prefabMaps[hash];
            var e = EntityManager.Instantiate(buffer[prefabindex].prefab);
            return e;
        }

        private void OnSpawnPrefabs(string GUID, NativeArray<Entity> container)
        {
            Hash128 hash = new Hash128(GUID);
            if (SystemAPI.ManagedAPI.TryGetSingleton<PoolMap>(out var poolMap) == false)
            {
                return;
            }
            if (poolMap.prefabMaps.ContainsKey(hash) == false)
            {
                return;
            }

            DynamicBuffer<PrefabPool> buffer = SystemAPI.GetSingletonBuffer<PrefabPool>();
            int prefabindex = poolMap.prefabMaps[hash];
            EntityManager.Instantiate(buffer[prefabindex].prefab, container);
        }

        private Entity OnSpawnPrefabWithID(int index)
        {
            DynamicBuffer<OrderedPrefabPool> buffer = SystemAPI.GetSingletonBuffer<OrderedPrefabPool>();
            var e = EntityManager.Instantiate(buffer[index].prefab);
            return e;
        }

        private void OnSpawnPrefabsWithID(int index, NativeArray<Entity> container)
        {
            DynamicBuffer<OrderedPrefabPool> buffer = SystemAPI.GetSingletonBuffer<OrderedPrefabPool>();
            EntityManager.Instantiate(buffer[index].prefab, container);
        }


        protected override void OnUpdate()
        {

        }
    }
}
