using UnityEngine;
using Unity.Entities;
namespace DarkLordGame
{
    public class FlowFieldAgentAuthoring : MonoBehaviour
    {
        public LayerMaskFlag layer;
    }

    public class FlowFieldAgentBaker : Baker<FlowFieldAgentAuthoring>
    {
        public override void Bake(FlowFieldAgentAuthoring authoring)
        {
            var e = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(e, new FlowFieldAgent { layerMask = authoring.layer });
        }
    }
}