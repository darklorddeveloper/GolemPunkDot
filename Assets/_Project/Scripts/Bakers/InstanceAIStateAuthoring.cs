using UnityEngine;
using Unity.Entities;
using Unity.Collections;
namespace DarkLordGame
{
    [RequireComponent(typeof(TopdownCharacterAuthoring))]
    public class InstanceAIStateAuthoring : StructAuthorizer<InstanceAIState>
    {
        public InstanceAIStateSettingBlob blob;
    }

    public class InstanceAIStateBaker : StructBaker<InstanceAIStateAuthoring, InstanceAIState>
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

            var builder = new BlobBuilder(Allocator.Temp);
            ref InstanceAIStateSettingBlob root = ref builder.ConstructRoot<InstanceAIStateSettingBlob>();
            root = authoring.blob;
            var blobRef = builder.CreateBlobAssetReference<InstanceAIStateSettingBlob>(Allocator.Persistent);
            AddBlobAsset(ref blobRef, out var _);
            AddComponent(e, new InstanceAIStateSetting{ setting = blobRef});
            builder.Dispose();
        }
    }
}
