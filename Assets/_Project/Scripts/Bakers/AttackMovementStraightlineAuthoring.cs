using UnityEngine;
namespace DarkLordGame
{
    [RequireComponent(typeof(SpawnerAuthoring), typeof(SafeDestroyAuthoring))]
    public class AttackMovementStraightlineAuthoring : StructAuthorizer<Attack, AttackMovementStraightline, MovementSpeed, HitEffect>
    {

    }

    public class AttackMovementStraightlineBaker : StructBaker<AttackMovementStraightlineAuthoring, Attack, AttackMovementStraightline, MovementSpeed, HitEffect>
    {
    }
}
