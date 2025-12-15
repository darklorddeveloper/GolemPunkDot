using UnityEngine;

namespace DarkLordGame
{
    [RequireComponent(typeof(AttackRequestDataAuthoring))]
    public class InstanceAIAttackAuthoring : StructAuthorizer<InstanceAIAttack>
    {
    }

    public class InstanceAIAttackBaker: StructBaker<InstanceAIAttackAuthoring, InstanceAIAttack>
    {
        
    }
}
