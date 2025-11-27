using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public struct Spawner : IComponentData
    {
        public Entity spawner;
    }
}
