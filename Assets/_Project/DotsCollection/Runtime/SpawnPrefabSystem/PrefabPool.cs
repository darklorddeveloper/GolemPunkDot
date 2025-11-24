using Unity.Entities;
using System.Collections.Generic;

namespace DarkLordGame
{
    [InternalBufferCapacity(10)]
    public struct PrefabPool : IBufferElementData
    {
        public Hash128 hash;
        public Entity prefab;
    }

    [InternalBufferCapacity(10)]
    public struct OrderedPrefabPool : IBufferElementData
    {
        public Entity prefab;
    }

    [System.Serializable]
    public class PoolMap : IComponentData
    {
        public Dictionary<Hash128, int> prefabMaps = new Dictionary<Hash128, int>();
    }

    public struct SetupPool : IComponentData, IEnableableComponent
    {

    }
}