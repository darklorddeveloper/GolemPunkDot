using UnityEngine;
using Unity.Entities;
namespace DarkLordGame
{
    public class FlowFieldAgentAuthoring : MonoBehaviour
    {
        public int layer;
    }

    public class FlowFieldAgentBaker : Baker<FlowFieldAgentAuthoring>
    {
        public override void Bake(FlowFieldAgentAuthoring authoring)
        {
            var e = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(e, new FlowFieldAgent { layer = authoring.layer });
        }
    }
}