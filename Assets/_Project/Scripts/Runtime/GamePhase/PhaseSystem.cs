using Unity.Entities;

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
                    state.EntityManager.SetComponentEnabled<GamePhaseIntro>(e, phase == PhaseType.GamePhase);
                    state.EntityManager.SetComponentEnabled<MinePhase>(e, phase == PhaseType.HomeStandbyphase);
                    state.EntityManager.SetComponentEnabled<SettingPhase>(e, phase == PhaseType.SettingPhase);
                    CameraMovement targetMovement = default;
                    switch (phase)
                    {
                        case PhaseType.BootStrap:
                            targetMovement = state.EntityManager.GetComponentData<BootStrapPhase>(e).targetCameraMovement;
                            break;
                        case PhaseType.Intro:
                            targetMovement = state.EntityManager.GetComponentData<IntroPhase>(e).targetCameraMovement;
                            break;
                        case PhaseType.GamePhase:
                            targetMovement = state.EntityManager.GetComponentData<GamePhase>(e).targetCameraMovement;
                            break;
                        case PhaseType.HomeStandbyphase:
                            targetMovement = state.EntityManager.GetComponentData<MinePhase>(e).targetCameraMovement;
                            break;
                        case PhaseType.SettingPhase:
                            targetMovement = state.EntityManager.GetComponentData<SettingPhase>(e).targetCameraMovement;
                            break;
                    }
                    SystemAPI.SetSingleton(targetMovement);
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
