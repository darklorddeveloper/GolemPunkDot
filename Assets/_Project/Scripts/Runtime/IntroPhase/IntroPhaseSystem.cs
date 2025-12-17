using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public partial class IntroPhaseSystem : SystemBase
    {
        private EntityQuery targetQuery;
        private float timeCount = 0;
        private const float WaitPeriod = 0.5f;
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
                if (phase.isChangingPhase)
                {
                    timeCount = 0;
                }
                timeCount += SystemAPI.Time.DeltaTime;
                if (timeCount < WaitPeriod) return;

                if (Singleton.instance.playerSaveData.playCount == 0)
                {
                    phase.phase = PhaseType.GamePhaseIntro;
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
