using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    
    public partial class BootStrapPhaseSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            Singleton.Init();
            RequireForUpdate<BootStrapPhase>();
        }
        protected override void OnUpdate()
        {
            var currentPhase = SystemAPI.GetSingleton<CurrentPhase>();
            currentPhase.phase = PhaseType.Intro;
            SystemAPI.SetSingleton(currentPhase);
        }
    }
}
