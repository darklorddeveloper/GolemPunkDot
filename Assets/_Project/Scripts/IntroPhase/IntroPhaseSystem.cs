using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public partial class IntroPhaseSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<IntroPhase>();
        }

        protected override void OnUpdate()
        {
            //check the animation is done. --- intro only
            // play the current page
            // fade out then next page
            // fadeout last page then go to gamephase
            if (SystemAPI.TryGetSingleton<CurrentPhase>(out var phase))
            {
                if (Singleton.instance.playerSaveData.playCount == 0)
                {
                    phase.phase = PhaseType.GamePhase;
                }
                else
                {
                    phase.phase = PhaseType.HomeStandbyphase;
                }
            }
        }
    }
}
