using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class UnitAuthoring : StructAuthorizer<Unit, ChangeMovementSpeed, Damage>
    {
        [Header("Note - this will add safedestroy")]
        [Header("Death")]
        public bool canDeath = true;
        public GameObject deathEffect;

        [Header("TakeDamage")]
        public float takeDamageAnimationPeriod = 0.3f;

        public float destroyPeriod = 0.0f;
    }

    public class UnitBaker : StructBaker<UnitAuthoring, Unit, ChangeMovementSpeed, Damage>
    {
        public override void Bake(UnitAuthoring authoring)
        {
            var unit = authoring.data1;
            unit.canDeath = authoring.canDeath;
            authoring.data1 = unit;

            base.Bake(authoring);
            var e = GetEntity(TransformUsageFlags.Dynamic);
            SetComponentEnabled<Damage>(e, false);
            if (authoring.canDeath)
            {
                AddComponent<Death>(e);
                SetComponentEnabled<Death>(e, false);
                if (authoring.deathEffect != null)
                {
                    var fx = GetEntity(authoring.deathEffect, TransformUsageFlags.Dynamic);
                    AddComponent(e, new DeathEffect { prefab = fx });
                }
                else
                {
                    AddComponent(e, new DeathEffect { prefab = Entity.Null });
                }
                AddComponent(e, new SafeDestroyComponent { period = authoring.destroyPeriod });
                AddComponent(e, new DestroyImmediate { destroyChild = true });
                SetComponentEnabled<SafeDestroyComponent>(e, false);
                SetComponentEnabled<DestroyImmediate>(e, false);
            }
            else
            {
                AddComponent(e, new DeathEffect { prefab = Entity.Null });
            }

            AddComponent(e, new TakeDamageAnimationPeriod { period = authoring.takeDamageAnimationPeriod });
        }
    }
}
