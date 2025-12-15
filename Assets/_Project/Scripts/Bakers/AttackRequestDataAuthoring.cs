using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class AttackRequestDataAuthoring : StructAuthorizer<AttackRequestData>
    {
        public GameObject prefab;
    }

    public class AttackRequestDataBaker : Baker<AttackRequestDataAuthoring>
    {
        public override void Bake(AttackRequestDataAuthoring authoring)
        {
            var e = GetEntity(Unity.Entities.TransformUsageFlags.Dynamic);
            var dat = authoring.data1;
            dat.attacker = e;
            if (authoring.prefab != null)
            {
                dat.prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic);
            }
            else
            {
                dat.prefab = Entity.Null;
            }
            AddComponent(e, dat);
            SetComponentEnabled<AttackRequestData>(e, false);
        }
    }
}
