using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class UnitAuthoring : StructAuthorizer<Unit, ChangeMovementSpeed>
    {
        public bool canDeath = true;
        public GameObject deathEffect;

    }

    public class UnitBaker : StructBaker<UnitAuthoring, Unit, ChangeMovementSpeed>
    {
        public override void Bake(UnitAuthoring authoring)
        {
            base.Bake(authoring);
            var e = GetEntity(TransformUsageFlags.Dynamic);
            if (authoring.canDeath)
            {
                AddComponent<Death>(e);
                SetComponentEnabled<Death>(e, false);
                if (authoring.deathEffect != null)
                {
                    var fx = GetEntity(authoring.deathEffect, TransformUsageFlags.Dynamic);
                    AddComponent(e, new DeathEffect { prefab = fx });
                }
            }
        }
    }
}
