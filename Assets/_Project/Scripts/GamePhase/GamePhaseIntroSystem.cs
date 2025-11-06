using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    public partial class GamePhaseIntroSystem : SystemBase
    {

        private EntityQuery targetQuery;
        private IEnumerator introEnumberator;
        private const float introPeriod = 0.5f;
        protected override void OnCreate()
        {
            base.OnCreate();
            base.OnCreate();
            targetQuery = SystemAPI.QueryBuilder()
            .WithAll<GamePhaseIntro>()
            .Build();
        }

        protected override void OnUpdate()
        {
            if (targetQuery.IsEmpty) return;

            var currentPhase = SystemAPI.GetSingleton<CurrentPhase>();
            var e = SystemAPI.GetSingletonEntity<CurrentPhase>();
            if (currentPhase.isChangingPhase)
            {
                //do intiialize golem and start ienumerator
                var id = Singleton.instance.playerSaveData.selectedClassID;
                var collection = SystemAPI.GetSingletonBuffer<GolemClassCollection>();
                var entity = EntityManager.Instantiate(collection[id].prefab);
                introEnumberator = Intro(entity);
            }

            if (introEnumberator == null)
            {
                return;
            }

            if (introEnumberator.MoveNext())
            {
                return;
            }

            EntityManager.SetComponentEnabled<GamePhaseIntro>(e, false);
            introEnumberator = null;
        }

        private IEnumerator Intro(Entity entity)
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
            transform.Position = startPos.targetPosition;
            EntityManager.SetComponentData(entity, transform);
        }
    }
}
