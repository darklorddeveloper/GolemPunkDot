using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public struct GamePhaseIntro : IComponentData
    {
        public float delayed;
        public float period;
    }
}
