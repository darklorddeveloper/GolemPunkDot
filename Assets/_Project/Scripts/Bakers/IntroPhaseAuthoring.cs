using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class IntroPhaseAuthoring : EnableStructAuthorizer<IntroPhase>
    {
        
    }

    public class IntroPhaseBaker : EnableStructBaker<IntroPhaseAuthoring, IntroPhase>
    {
    }
    

}
