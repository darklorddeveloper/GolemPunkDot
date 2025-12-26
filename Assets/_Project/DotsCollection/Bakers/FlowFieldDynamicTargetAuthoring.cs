using UnityEngine;
using Unity.Entities;
namespace DarkLordGame
{
    public class FlowFieldDynamicTargetAuthoring : StructAuthorizer<DynamicFlowFieldTarget>
    {

    }
    public class FlowFieldDynamicTargetBaker : StructBaker<FlowFieldDynamicTargetAuthoring, DynamicFlowFieldTarget>
    {

    }
}