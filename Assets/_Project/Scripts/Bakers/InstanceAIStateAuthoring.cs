using UnityEngine;

namespace DarkLordGame
{
    [RequireComponent(typeof(TopdownCharacterAuthoring))]
    public class InstanceAIStateAuthoring : StructAuthorizer<InstanceAIState>
    {
    }

    public class InstanceAIStateBaker: StructBaker<InstanceAIStateAuthoring, InstanceAIState>
    {
        public override void Bake(InstanceAIStateAuthoring authoring)
        {
            base.Bake(authoring);
            var e = GetEntity(Unity.Entities.TransformUsageFlags.Dynamic);
            AddComponent<InstanceAIStateChanged>(e);
            AddComponent<InstanceAIStateFlagIdle>(e);
            AddComponent<InstanceAIStateFlagMove>(e);
            AddComponent<InstanceAIStateFlagAttack>(e);
            AddComponent<InstanceAIStateFlagTakeDamage>(e);
            SetComponentEnabled<InstanceAIStateFlagIdle>(e, false);
            SetComponentEnabled<InstanceAIStateFlagMove>(e, false);
            SetComponentEnabled<InstanceAIStateFlagAttack>(e, false);
            SetComponentEnabled<InstanceAIStateFlagTakeDamage>(e, false);
        }
    }
}
