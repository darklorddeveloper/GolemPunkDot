using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace DarkLordGame
{
    public struct DamageReaction : IBufferElementData
    {
        public Entity target;
    }
}
