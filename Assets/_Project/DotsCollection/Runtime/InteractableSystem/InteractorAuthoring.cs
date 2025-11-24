using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class InteractorAuthoring : StructAuthorizer<Interactor>
    {
    }

    public class InteractorBaker : StructBaker<InteractorAuthoring, Interactor> { }

    [System.Serializable]
    public struct Interactor : IComponentData
    {
        public float radius;
        public uint interactLayerBit;
    }
}
