using UnityEngine;
using Unity.Entities;

namespace DarkLordGame
{
    [RequireComponent(typeof(SpawnerAuthoring), typeof(SafeDestroyAuthoring))]
    public class AttackMovementMeleeAuthoring : StructAuthorizer<AttackMovementMelee, HitEffect, Attack>
    {
        public GameObject hitEffect;
    }

    public class AttackMovementMeleeBaker : StructBaker<AttackMovementMeleeAuthoring, AttackMovementMelee, HitEffect, Attack>
    {
        public override void Bake(AttackMovementMeleeAuthoring authoring)
        {
            base.Bake(authoring);
            var e = GetEntity(TransformUsageFlags.Dynamic);
            var prefab = GetEntity(authoring.hitEffect, TransformUsageFlags.Dynamic);
            SetComponent(e, new HitEffect { entity = prefab });
        }
    }
}
