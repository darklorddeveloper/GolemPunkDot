using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public partial struct PhaseSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CurrentPhase>();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (SystemAPI.TryGetSingleton<CurrentPhase>(out var currentPhase))
            {
                if (currentPhase.previousPhase != currentPhase.phase)
                {
                    currentPhase.isChangingPhase = true;
                    currentPhase.previousPhase = currentPhase.phase;
                    var e = SystemAPI.GetSingletonEntity<CurrentPhase>();
                    var phase = currentPhase.phase;
                    state.EntityManager.SetComponentEnabled<BootStrapPhase>(e, phase == PhaseType.BootStrap);
                    state.EntityManager.SetComponentEnabled<IntroPhase>(e, phase == PhaseType.Intro);
                    state.EntityManager.SetComponentEnabled<GamePhase>(e, phase == PhaseType.GamePhase);
                    state.EntityManager.SetComponentEnabled<MinePhase>(e, phase == PhaseType.MinePhase);
                    state.EntityManager.SetComponentEnabled<SettingPhase>(e, phase == PhaseType.SettingPhase);

                    SystemAPI.SetSingleton(currentPhase);
                }
                else
                {
                    if (currentPhase.isChangingPhase)
                    {
                        currentPhase.isChangingPhase = false;
                        SystemAPI.SetSingleton(currentPhase);
                    }
                }
            }
        }
    }
}
