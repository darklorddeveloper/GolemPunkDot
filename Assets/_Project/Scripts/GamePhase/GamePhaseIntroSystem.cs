using System.Collections;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public partial class GamePhaseIntroSystem : SystemBase
    {
        private IEnumerator introEnumberator;
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<GamePhaseIntro>();
        }

        protected override void OnUpdate()
        {
            var currentPhase = SystemAPI.GetSingleton<CurrentPhase>();
            if (currentPhase.isChangingPhase)
            {
                //do intiialize golem and start ienumerator
            }

            if (introEnumberator == null)
            {
                return;
            }

            if (introEnumberator.MoveNext())
            {
                return;
            }
            var e = SystemAPI.GetSingletonEntity<GamePhaseIntro>();
            EntityManager.SetComponentEnabled<GamePhaseIntro>(e, false);
            introEnumberator = null;
        }
    }
}
