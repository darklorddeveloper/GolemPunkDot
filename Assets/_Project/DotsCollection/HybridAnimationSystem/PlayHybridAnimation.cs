using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class PlayHybridAnimation : IComponentData, IEnableableComponent
    {
        public string animationName;
        public float period;
    }
}
