using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class WallPositionAuthoring : StructAuthorizer<WallPosition>
    {
    }

    public class WallPositionBaker : Baker<WallPositionAuthoring>
    {
        public override void Bake(WallPositionAuthoring authoring)
        {
            var e = GetEntity(TransformUsageFlags.None);
            AddComponent(e, new WallPosition { position = authoring.transform.position });
        }
    }
}
