using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{

    [System.Serializable]
    public struct CameraShake : IComponentData, IEnableableComponent
    {
        public float period;
        public float power;
    }
}
