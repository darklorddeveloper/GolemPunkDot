using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;

namespace DarkLordGame
{
    public class DamageReactionAuthoring : MonoBehaviour
    {
        public List<GameObject> targets = new();
        public class Baker : Baker<DamageReactionAuthoring>
        {
            public override void Bake(DamageReactionAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                if (authoring.targets.Count <= 0) return;
                var buff = AddBuffer<DamageReaction>(entity);
                for (int i = 0, length = authoring.targets.Count; i < length; i++)
                {
                    var e = GetEntity(authoring.targets[i], TransformUsageFlags.Dynamic);
                    buff.Add(new DamageReaction { target = e });
                }
            }
        }
    }
}