using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class UnitAuthoring : StructAuthorizer<Unit, ChangeMovementSpeed, Damage>
    {
        public bool canDeath = true;
        public GameObject deathEffect;

        public bool addSafeDestroyComponent;
        public bool destroyChild;
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
                if (authoring.addSafeDestroyComponent)
                {
                    AddComponent<SafeDestroyComponent>(e);
                    AddComponent(e, new DestroyImmediate { destroyChild = authoring.destroyChild });
                    SetComponentEnabled<SafeDestroyComponent>(e, false);
                    SetComponentEnabled<DestroyImmediate>(e, false);
                }
            }
            else
            {
                AddComponent(e, new DeathEffect { prefab = Entity.Null });
            }
        }
    }
}
