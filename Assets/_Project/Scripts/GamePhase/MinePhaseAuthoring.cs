using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class MinePhaseAuthoring : EnableStructAuthorizer<MinePhase>
    {
        
    }
    
    public class MinePhaseBaker : EnableStructBaker<MinePhaseAuthoring, MinePhase>
    {
    }
    public struct MinePhase: IComponentData, IEnableableComponent
    {
    }
}
