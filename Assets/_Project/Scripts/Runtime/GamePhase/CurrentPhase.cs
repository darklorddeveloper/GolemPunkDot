using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{

    public enum PhaseType
    {
        BootStrap,
        Intro, // do cinematic intro
        GamePhase,
        GamePhaseIntro,
        SettingPhase,
        HomeStandbyphase,
        Result,
    }
    
    [System.Serializable]
    public struct CurrentPhase : IComponentData
    {
        public PhaseType phase;
        public PhaseType previousPhase;
        public bool isChangingPhase;
    }
}