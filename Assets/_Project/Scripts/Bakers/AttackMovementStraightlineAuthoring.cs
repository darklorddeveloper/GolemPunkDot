using UnityEngine;
using Unity.Entities;
namespace DarkLordGame
{
    [RequireComponent(typeof(SpawnerAuthoring), typeof(SafeDestroyAuthoring))]
    public class AttackMovementStraightlineAuthoring : StructAuthorizer<Attack, AttackMovementStraightline, MovementSpeed, HitEffect>
    {
        public GameObject hitEffect;

    }

    public class AttackMovementStraightlineBaker : StructBaker<AttackMovementStraightlineAuthoring, Attack, AttackMovementStraightline, MovementSpeed, HitEffect>
    {
        public override void Bake(AttackMovementStraightlineAuthoring authoring)
        {
            base.Bake(authoring);
            var e = GetEntity(TransformUsageFlags.Dynamic);
            var prefab = GetEntity(authoring.hitEffect, TransformUsageFlags.Dynamic);
            SetComponent(e, new HitEffect { entity = prefab });
        }
    }
}
