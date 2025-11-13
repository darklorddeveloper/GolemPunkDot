using System;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    //pass bonus buff or runes effects into attack request multiply it
    //pass bonus enemies attack
    public partial class UnitAttackSystem : SystemBase
    {
        public static Action<Entity, Entity, AttackRequestData> onUnitAttack;

        protected override void OnCreate()
        {
            base.OnCreate();
            onUnitAttack = OnUnitAttack;
            Enabled = false;
        }

        private void OnUnitAttack(Entity attacker, Entity prefab, AttackRequestData data)
        {
            data.attacker = attacker;
            data.prefab = prefab;
            EntityManager.SetComponentData(attacker, data);
            EntityManager.SetComponentEnabled<AttackRequestData>(attacker, true);
        }

        protected override void OnUpdate()
        {

        }
    }
}
