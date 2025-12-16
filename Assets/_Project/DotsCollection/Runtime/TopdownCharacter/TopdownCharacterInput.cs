using Unity.Entities;
using Unity.Mathematics;

namespace DarkLordGame
{

    [System.Serializable]
    public struct TopdownCharacterInput : IComponentData, IEnableableComponent
    {
        public float3 movement;
        public float3 lookAtTargetPoint;//world position
        // public ushort bits;
        public bool dashAction;
        public bool isHoldingDashAction;
        public bool primaryAction, secondaryAction;
        public bool isHoldingPrimaryAction, isHoldingSecondaryAction;
        public bool skill1, skill2, skill3, skill4;
        public bool isHoldingSkill1, isHoldingSkill2;
        public bool isHoldingSkill3, isHoldingSkill4;
    }

    // public static class TopdownCharacterInputBitAddress
    // {
    //     public const ushort dash = 1 << 0;
    //     public const ushort isHoldingDashAction = 1 << 1;
    //     public const ushort primaryAction = 1 << 2;
    //     public const ushort secondaryAction = 1 << 3;
    //     public const ushort isHoldingPrimaryAction = 1 << 4, isHoldingSecondaryAction = 1 << 5;
    //     public const ushort skill1 = 1 << 6, skill2 = 1 << 7, skill3 = 1 << 8, skill4 = 1 << 9;
    //     public const ushort isHoldingSkill1 = 1 << 10, isHoldingSkill2 = 1 << 11;
    //     public const ushort isHoldingSkill3 = 1 << 12, isHoldingSkill4 = 1 << 13;
    // }

}
