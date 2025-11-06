using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public partial class IntroPhaseSystem : SystemBase
    {
        private EntityQuery targetQuery;
        protected override void OnCreate()
        {
            base.OnCreate();
            targetQuery = SystemAPI.QueryBuilder()
            .WithAll<IntroPhase>()
            .Build();
        }

        protected override void OnUpdate()
        {
            if (targetQuery.IsEmpty)
            {
                return;
            }
            //check the animation is done. --- intro only
            // play the current page
            // fade out then next page
            // fadeout last page then go to gamephase
            if (SystemAPI.TryGetSingleton<CurrentPhase>(out var phase))
            {
                Debug.Log("here is intro phase " + phase.phase + "  " + phase.previousPhase);
                if (Singleton.instance.playerSaveData.playCount == 0)
                {
                    phase.phase = PhaseType.GamePhase;
                }
                else
                {
                    phase.phase = PhaseType.HomeStandbyphase;
                }
                SystemAPI.SetSingleton(phase);
            }
        }
    }
}
