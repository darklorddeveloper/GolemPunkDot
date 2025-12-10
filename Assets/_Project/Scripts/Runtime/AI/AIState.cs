using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public enum AIStateType
    {
        Idle,
        Move,
        Attack,
        TakeDamage,
        Death,
        //Stunned,//expand later
    }

    public struct AIState : IComponentData
    {
        public AIStateType previousState, currentState;
        public float timeSinceStarted;
    }
}
