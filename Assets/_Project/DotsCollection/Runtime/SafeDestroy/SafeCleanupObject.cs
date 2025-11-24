using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class SafeCleanupObject : ICleanupComponentData
    {
        public GameObject mainGameObject;
    }
}
