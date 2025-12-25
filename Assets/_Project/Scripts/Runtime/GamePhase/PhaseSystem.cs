using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class PhaseData
    {
        public PhaseType phaseType;
        public List<Type> targetTypes;
    }
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial class PhaseSystem : SystemBase
    {
        private static bool initialized = false;

        private static List<Type> allTypes;
        public static List<PhaseData> datas = new List<PhaseData>
        {
             new PhaseData
             {
                phaseType = PhaseType.BootStrap,
                targetTypes = new List<Type>{typeof(BootStrapPhaseSystem)}
             },
             new PhaseData
             {
                 phaseType = PhaseType.Intro,
                 targetTypes = new List<Type>{typeof(IntroPhaseSystem)}
             },
             new PhaseData
             {
                phaseType = PhaseType.GamePhase,
                targetTypes = new List<Type>
                {
                  typeof(DamageSystem),
                  typeof(TopdownPlayerInputSystem),
                  typeof(TopdownCharacterMovementSystem),
                  typeof(InstanceAIStateSystem),
                  //wavephase
                  //GamePhase
                }
             },
             new PhaseData
             {
                phaseType = PhaseType.GamePhaseIntro,
                targetTypes = new List<Type>
                {
                  typeof(GamePhaseIntroSystem)
                }
             },
             new PhaseData
             {
                phaseType = PhaseType.SettingPhase,
                targetTypes = new List<Type>
                {
                    //setting phase only
                }
             }

        };

        public static void BuildSystemMaps(World world)
        {
            allTypes = new();

            for (int i = 0, length = datas.Count; i < length; i++)
            {
                var t = datas[i].targetTypes;
                for (int j = 0, num = t.Count; j < num; j++)
                {
                    if (allTypes.Contains(t[j]))
                    {
                        continue;
                    }
                    allTypes.Add(t[j]);
                }
            }

        }

        protected override void OnUpdate()
        {
            if (SystemAPI.TryGetSingleton<CurrentPhase>(out var currentPhase) == false)
            {
                return;
            }
            if (currentPhase.previousPhase == currentPhase.phase)
            {
                if (currentPhase.isChangingPhase)
                {
                    currentPhase.isChangingPhase = false;
                    SystemAPI.SetSingleton(currentPhase);
                }
                return;
            }
            if (initialized == false)
                BuildSystemMaps(CheckedStateRef.World);
            var phase = currentPhase.phase;
            var state = CheckedStateRef;
            var e = SystemAPI.GetSingletonEntity<CurrentPhase>();
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
                case PhaseType.GamePhaseIntro:
                    targetMovement = state.EntityManager.GetComponentData<GamePhase>(e).targetCameraMovement;
                    break;
                case PhaseType.HomeStandbyphase:
                    targetMovement = state.EntityManager.GetComponentData<MinePhase>(e).targetCameraMovement;
                    break;
                case PhaseType.SettingPhase:
                    targetMovement = state.EntityManager.GetComponentData<SettingPhase>(e).targetCameraMovement;
                    break;
                    case PhaseType.Result:
                    targetMovement = state.EntityManager.GetComponentData<SettingPhase>(e).targetCameraMovement;
                    break;
            }

            currentPhase.previousPhase = currentPhase.phase;
            SetEnableSystems(allTypes, false);
            for (int i = 0, length = datas.Count; i < length; i++)
            {
                if (datas[i].phaseType == currentPhase.phase)
                {
                    SetEnableSystems(datas[i].targetTypes, true);
                }
            }
            SystemAPI.SetSingleton(targetMovement);//camera movement of each phase
            SystemAPI.SetSingleton(currentPhase);
        }

        private void SetEnableSystems(List<Type> systemTypes, bool enable)
        {
            var state = CheckedStateRef;
            foreach (var type in systemTypes)
            {
                if (typeof(ISystem).IsAssignableFrom(type))
                {
                    var handle = state.World.Unmanaged.GetExistingUnmanagedSystem(type);
                    if (handle == default) continue;

                    ref var sysState = ref state.World.Unmanaged.ResolveSystemStateRef(handle); // non-generic :contentReference[oaicite:3]{index=3}
                    sysState.Enabled = enable;
                    continue;
                }

                if (typeof(ComponentSystemBase).IsAssignableFrom(type))
                {
                    var managed = state.World.GetExistingSystemManaged(type) as ComponentSystemBase;
                    if (managed == null) continue;

                    managed.Enabled = enable;
                    continue;
                }

            }
        }
    }
}
