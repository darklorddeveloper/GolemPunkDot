using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Entities;

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

    [BurstCompile]
    public static class InstanceAIStateDataUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static InstanceAIStateData GetStateData(in InstanceAIStateSettingBlob setting, InstanceAIStateType stateType)
        {
            switch (stateType)
            {
                case InstanceAIStateType.Idle:
                    return setting.idleState;
                case InstanceAIStateType.Move:
                    return setting.moveState;
                case InstanceAIStateType.Attack:
                    return setting.attackState;
                case InstanceAIStateType.TakeDamage:
                    return setting.takeDamageState;
                default:
                    return setting.idleState;
            }
        }
    }

    [System.Serializable]
    public struct InstanceAIState : IComponentData
    {
        public float timeSinceStarted;
        public float storedDamage;
        public int loopCount;//0 
        public InstanceAIStateType currentStateType;
        public bool isInterupted;
        public InstanceAIStateType inturuptStateType;
    }

    public struct InstanceAIStateSetting : IComponentData
    {
        public BlobAssetReference<InstanceAIStateSettingBlob> setting;
    }

    [System.Serializable]
    public struct InstanceAIStateSettingBlob
    {
        public short interupDamage;
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
