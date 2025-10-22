using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public enum PhaseType
    {
        BootStrap,
        Intro, // do cinematic intro
        GamePhase,
        SettingPhase,
        MinePhase,
    }

    public class CurrentPhaseAuthoring : StructAuthorizer<CurrentPhase>
    {

    }

    public class CurrentPhaseBaker : StructBaker<CurrentPhaseAuthoring, CurrentPhase>
    {
    }

    [System.Serializable]
    public struct CurrentPhase : IComponentData
    {
        public PhaseType phase;
        public PhaseType previousPhase;
        public bool isChangingPhase;
    }
}
