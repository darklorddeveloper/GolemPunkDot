using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class UnitAuthoring : StructAuthorizer<Unit, Death, ChangeMovementSpeed>
    {
        public GameObject deathEffect;

    }

    public class UnitBaker : StructBaker<UnitAuthoring, Unit, Death, ChangeMovementSpeed>
    {
        public override void Bake(UnitAuthoring authoring)
        {
            base.Bake(authoring);
            var e = GetEntity(TransformUsageFlags.Dynamic);
            SetComponentEnabled<Death>(e, false);
            if (authoring.deathEffect != null)
            {
                var fx = GetEntity(authoring.deathEffect, TransformUsageFlags.Dynamic);
                AddComponent(e, new DeathEffect { prefab = fx });
            }
        }
    }
}
