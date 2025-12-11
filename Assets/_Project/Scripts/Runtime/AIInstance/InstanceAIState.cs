using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public enum InstanceAIStateType
    {
        Idle,
        Move,
        Attack,
        TakeDamage,
    }

    [System.Serializable]
    public struct InstanceAIStateData
    {
        public InstanceAnimationID animationIndex;
        public InstanceAIStateType stateType;
        public float stateMaxPeriod;//max because some state will manually do conditioning
        public InstanceAIStateType nextState;
        public int loop;//0 default
        public float loopInterval;//repeat usually before it finished
    }

    [System.Serializable]
    public struct InstanceAIState : IComponentData
    {
        public float timeSinceStarted;
        public int loopCount;//0 
        public float interupDamage;
        public float storedDamage;
        public InstanceAIStateData currentStateData;
        public bool isInterupted;
        public InstanceAIStateData interuptState;
        public InstanceAIStateData idleState, moveState, attackState, takeDamageState;
    }

    public struct InstanceAIStateChanged : IComponentData, IEnableableComponent
    {
        
    }
    public struct InstanceAIStateFlagIdle : IComponentData, IEnableableComponent
    {
        
    }

    public struct InstanceAIStateFlagMove : IComponentData, IEnableableComponent
    {
        
    }

    public struct InstanceAIStateFlagAttack : IComponentData, IEnableableComponent
    {   
    }

    public struct InstanceAIStateFlagTakeDamage : IComponentData, IEnableableComponent
    {
    }
}
