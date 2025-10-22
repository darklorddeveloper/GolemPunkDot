using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class BootStrapPhaseAuthoring : EnableStructAuthorizer<BootStrapPhase>
    {
        
    }
    
    public class BootStrapPhaseBaker : EnableStructBaker<BootStrapPhaseAuthoring, BootStrapPhase>
    {
    }
    

    public struct BootStrapPhase : IComponentData, IEnableableComponent
    {
        
    }
}
