using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    public partial class GamePhaseSystem : SystemBase
    {
        private EntityQuery enemyQuery;
        private EntityQuery playerQuery;

        private bool isGameFinished = false;
        protected override void OnCreate()
        {
            base.OnCreate();
            playerQuery = SystemAPI.QueryBuilder().WithAll<PlayerComponent>().Build();
            enemyQuery = SystemAPI.QueryBuilder().WithAll<EnemyComponent>().Build();
        }

        protected override void OnUpdate()
        {
            if (SystemAPI.TryGetSingleton<CurrentPhase>(out var currentPhase))
            {
                if (currentPhase.isChangingPhase)
                {
                    var difficulty = Singleton.instance.resourcesData.difficultyDatas[Singleton.instance.playerSaveData.selectedDifficulty];
                    float3 pos = new float3(difficulty.distanceFromCenter, 0, 0);
                    var rot = quaternion.LookRotation(new float3(-1, 0, 0), new float3(0, 1, 0));
                    var e = SpawnPrefabSystem.SpawnPrefab(difficulty.difficultyPrefab);
                    EntityManager.SetComponentData(e, new LocalTransform { Position = pos, Rotation = rot, Scale = 1.0f });
                    isGameFinished = false;
                    return;
                }
            }
            if (isGameFinished) return;
            if (enemyQuery.IsEmpty)
            {
                isGameFinished = true;
                //all enemies are death
//victory
                return;
            }

            if (playerQuery.IsEmpty)
            {
                isGameFinished = true;
                return;
            }
        }
    }
}
