using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    public partial class GamePhaseIntroSystem : SystemBase
    {

        private IEnumerator introEnumberator;
        private float timeCount = 0;

        protected override void OnUpdate()
        {
            var currentPhase = SystemAPI.GetSingleton<CurrentPhase>();
            var e = SystemAPI.GetSingletonEntity<CurrentPhase>();
            if (currentPhase.isChangingPhase)
            {
                timeCount = 0;
            }

            var intro = EntityManager.GetComponentData<GamePhaseIntro>(e);
            if (timeCount <= intro.delayed)
            {
                timeCount += SystemAPI.Time.DeltaTime;
                if (timeCount > intro.delayed)
                {
                    //do intiialize golem and start ienumerator
                    var id = Singleton.instance.playerSaveData.selectedClassID;
                    var collection = SystemAPI.GetSingletonBuffer<GolemClassCollection>();
                    var entity = EntityManager.Instantiate(collection[id].prefab);
                    introEnumberator = Intro(entity, intro.period);
                }
            }

            if (introEnumberator == null)
            {
                return;
            }

            if (introEnumberator.MoveNext())
            {
                return;
            }
            currentPhase.phase = PhaseType.GamePhase;
            SystemAPI.SetSingleton(currentPhase);
            introEnumberator = null;
        }

        private IEnumerator Intro(Entity entity, float introPeriod)
        {
            float t = 0;
            var transform = EntityManager.GetComponentData<LocalTransform>(entity);
            var startPos = SystemAPI.GetSingleton<GolemStartPosition>();
            transform.Position = startPos.position;
            EntityManager.SetComponentData(entity, transform);
            while (t < introPeriod)
            {
                transform.Position = math.lerp(startPos.position, startPos.targetPosition, t / introPeriod);
                EntityManager.SetComponentData(entity, transform);
                t += SystemAPI.Time.DeltaTime;
                yield return null;
            }

            //play story here
            //check skip
            transform.Position = startPos.targetPosition;
            EntityManager.SetComponentData(entity, transform);
        }
    }
}
