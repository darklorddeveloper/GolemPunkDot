using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public struct InstanceAIAttack : IComponentData
    {
        public float delayed;
        public bool attacked;
    }
}
