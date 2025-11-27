using System;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    //pass bonus buff or runes effects into attack request multiply it
    //Enemy global stats increase in spawn time. No affect in runtime only player units are complex
    public partial class GolemAttackSystem : SystemBase
    {
        public static Action<Entity, string, AttackRequestData> onGolemAttack;

        protected override void OnCreate()
        {
            base.OnCreate();
            onGolemAttack = OnGolemAttack;
            Enabled = false;
        }

        private void OnGolemAttack(Entity attacker, string prefabPath, AttackRequestData data)
        {
            Entity prefab = SpawnPrefabSystem.GetPrefab(prefabPath);

            data.attacker = attacker;
            data.prefab = prefab;
            var golem = EntityManager.GetComponentObject<GolemEntity>(attacker).golem;
            golem.currentRequestData = data;
            if (golem.activatingPart != null) //normally not null
            {
                for (int i = 0; i < golem.activatingPart.runes.Count; i++)
                {
                    if (golem.activatingPart.runes[i].effect.effectTiming == EffectTiming.OnAttack)
                    {
                        golem.activatingPart.runes[i].effect.OnActivate(attacker, EntityManager);
                    }
                }
            }
            EntityManager.SetComponentData(attacker, golem.currentRequestData);
            EntityManager.SetComponentEnabled<AttackRequestData>(attacker, true);
        }

        protected override void OnUpdate()
        {

        }
    }
}
