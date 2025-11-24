using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{

    [System.Serializable]
    public struct Interactor : IComponentData
    {
        public float radius;
        public uint interactLayerBit;
    }
}
