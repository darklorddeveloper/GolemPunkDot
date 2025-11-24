using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class SetupHybridAnimation : IComponentData, IEnableableComponent
    {
        public GameObject targetGameObject; // must have animator
    }
}
