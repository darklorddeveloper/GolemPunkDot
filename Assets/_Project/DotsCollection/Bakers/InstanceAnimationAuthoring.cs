using System.Collections.Generic;
using UnityEngine;

namespace DarkLordGame
{
    public class InstanceAnimationAuthoring : StructAuthorizer<PlayInstanceAnimation, CurrentInstanceAnimationIndex>
    {
        public bool hasStartTime = true;
        public bool hasDamageTime = true;
        
        public List<GameObject> targets = new();
    }
    public class InstanceAnimationBaker : StructBaker<InstanceAnimationAuthoring, PlayInstanceAnimation, CurrentInstanceAnimationIndex>
    {
        public override void Bake(InstanceAnimationAuthoring authoring)
        {
            base.Bake(authoring);
            var e = GetEntity(Unity.Entities.TransformUsageFlags.Dynamic);
            if (authoring.targets.Count <= 0) return;

            var buff = AddBuffer<InstanceAnimation>(e);
            for (int i = 0, length = authoring.targets.Count; i < length; i++)
            {
                var child = GetEntity(authoring.targets[i], Unity.Entities.TransformUsageFlags.Dynamic);
                buff.Add(new InstanceAnimation { target = child });
            }
            AddComponent<InstanceAnimationDelayedPlay>(e);
            SetComponentEnabled<InstanceAnimationDelayedPlay>(e, false);
        }
    }
}
