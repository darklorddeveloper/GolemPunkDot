using System.Collections;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [UpdateAfter(typeof(SetupClassSystem))]
    public partial class BootStrapPhaseSystem : SystemBase
    {
        private bool startedFadingIn = false;
        private IEnumerator runningFadeEnumerator;
        protected override void OnCreate()
        {
            base.OnCreate();
            Singleton.Init();
            RequireForUpdate<BootStrapPhase>();
        }

        protected override void OnUpdate()
        {
            var fadeLayer = SystemAPI.ManagedAPI.GetSingleton<FadeLayerContainer>();
            if (fadeLayer.initialized == false)
            {
                return;
            }

            var currentPhase = SystemAPI.GetSingleton<CurrentPhase>();

            if (startedFadingIn == false)
            {
                runningFadeEnumerator = fadeLayer.fadeLayer.Fade(1.0f, 0.0f, 3.0f);
                startedFadingIn = true;
            }
            if(runningFadeEnumerator != null && runningFadeEnumerator.MoveNext())
            {
                return;
            }
            currentPhase.phase = PhaseType.Intro;
            SystemAPI.SetSingleton(currentPhase);
        }
    }
}
