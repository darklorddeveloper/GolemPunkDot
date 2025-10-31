using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    public partial class TransformSyncSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var (transform, trans) in SystemAPI.Query<TransformSync, LocalTransform>())
            {
                if (transform.targetTransform == null) continue;
                transform.targetTransform.position = trans.Position;
                transform.targetTransform.rotation = trans.Rotation;
            }
        }
    }
}
