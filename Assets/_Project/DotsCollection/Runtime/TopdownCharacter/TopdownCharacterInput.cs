using Unity.Entities;
using Unity.Mathematics;

namespace DarkLordGame
{

    [System.Serializable]
    public struct TopdownCharacterInput : IComponentData, IEnableableComponent
    {
        public float3 movement;
        public float3 lookAtTargetPoint;//world position
        public bool dashAction;
        public bool isHoldingDashAction;
        public bool primaryAction, secondaryAction;
        public bool isHoldingPrimaryAction, isHoldingSecondaryAction;
        public bool skill1, skill2, skill3, skill4;
        public bool isHoldingSkill1, isHoldingSkill2;
        public bool isHoldingSkill3, isHoldingSkill4;
    }

}
