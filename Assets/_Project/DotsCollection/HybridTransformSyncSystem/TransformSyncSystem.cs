using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    public partial class TransformSyncSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var (transformSync, trans) in SystemAPI.Query<TransformSync, LocalTransform>())
            {
                if (transformSync.targetTransform == null) continue;
                transformSync.previousPosition = transformSync.targetTransform.position;
                transformSync.targetTransform.position = trans.Position;
                transformSync.deltaPosition = trans.Position - transformSync.previousPosition;
                transformSync.targetTransform.rotation = trans.Rotation;
            }
        }
    }
}
