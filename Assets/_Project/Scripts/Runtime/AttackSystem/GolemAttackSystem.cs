using System;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    //pass bonus buff or runes effects into attack request multiply it
    //Enemy global stats increase in spawn time. No affect in runtime only player units are complex
    public partial class GolemAttackSystem : SystemBase
    {
        public static Action<Entity, Entity, AttackRequestData> onGolemAttack;

        protected override void OnCreate()
        {
            base.OnCreate();
            onGolemAttack = OnUnitAttack;
            Enabled = false;
        }

        private void OnUnitAttack(Entity attacker, Entity prefab, AttackRequestData data)
        {
            data.attacker = attacker;
            data.prefab = prefab;

            var golem = EntityManager.GetComponentObject<GolemEntity>(attacker).golem;
            golem.currentRequestData = data;
            if(golem.activatingPart != null) //normally not null
            {
                for(int i = 0;  i < golem.activatingPart.runes.Count; i++)
                {
                    golem.activatingPart.runes[i].effect.OnActivate(attacker, EntityManager);
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
