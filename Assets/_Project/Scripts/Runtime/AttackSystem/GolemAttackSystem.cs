using System;
using Unity.Entities;

namespace DarkLordGame
{
    //pass bonus buff or runes effects into attack request multiply it
    //Enemy global stats increase in spawn time. No affect in runtime only player units are complex
    public partial class GolemAttackSystem : SystemBase
    {
        public static Action<Entity, string, GolemAttachPoint, AttackRequestData, bool> onGolemAttack;

        protected override void OnCreate()
        {
            base.OnCreate();
            onGolemAttack = OnGolemAttack;
            Enabled = false;
        }

        private void OnGolemAttack(Entity attacker, string prefabPath, GolemAttachPoint attachPoint, AttackRequestData data, bool canChainEffect)
        {
            Entity prefab = SpawnPrefabSystem.GetPrefab(prefabPath);

            data.attacker = attacker;
            data.prefab = prefab;
            // data.position = 
            var golem = EntityManager.GetComponentObject<GolemEntity>(attacker).golem;
            var point = golem.GetAttachPoint(GolemAttachPoint.AttackForwardPoint);
            data.position = point.position;
            data.rotation = point.rotation;
            golem.currentRequestData = data;
            if (canChainEffect) //normally not null
            {
                golem.ActiveAllEffects(attacker, EntityManager, EffectTiming.OnAttack);
            }
            EntityManager.SetComponentData(attacker, golem.currentRequestData);
            EntityManager.SetComponentEnabled<AttackRequestData>(attacker, true);
        }

        protected override void OnUpdate()
        {

        }
    }
}
