using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class SpawnAttackRequestAuthoring : MonoBehaviour
    {
        [Header("Must setup attack data inside the prefab")]
        public GameObject prefab;
        public SpawnRequestBlob blob;

    }

    public class SpawnAttackRequestBaker : Baker<SpawnAttackRequestAuthoring>
    {
        public override void Bake(SpawnAttackRequestAuthoring authoring)
        {
            var e = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent<SpawnAttackRequest>(e);
            SetComponentEnabled<SpawnAttackRequest>(e, false);

            AddComponent<SpawnAttackCounter>(e);
            //setting
            var builder = new BlobBuilder(Allocator.Temp);
            ref SpawnRequestBlob root = ref builder.ConstructRoot<SpawnRequestBlob>();
            root = authoring.blob;
            var blobRef = builder.CreateBlobAssetReference<SpawnRequestBlob>(Allocator.Persistent);
            AddBlobAsset(ref blobRef, out var _);
            
            var prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic);
            AddComponent(e, new SpawnAttackRequestSetting
            {
                setting = blobRef,
                prefab = prefab
            });
            builder.Dispose();
        }
    }
}
