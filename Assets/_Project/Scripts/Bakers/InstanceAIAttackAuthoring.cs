using UnityEngine;

namespace DarkLordGame
{
    [RequireComponent(typeof(SpawnAttackRequest))]
    public class InstanceAIAttackAuthoring : StructAuthorizer<InstanceAIAttack>
    {
    }

    public class InstanceAIAttackBaker: StructBaker<InstanceAIAttackAuthoring, InstanceAIAttack>
    {
        
    }
}
