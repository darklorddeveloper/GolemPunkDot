using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public struct DamageReaction : IBufferElementData
    {
        public Entity target;
    }
}
