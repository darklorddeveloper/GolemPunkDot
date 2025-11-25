using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{

    public class CurrentPhaseAuthoring : StructAuthorizer<CurrentPhase>
    {

    }

    public class CurrentPhaseBaker : StructBaker<CurrentPhaseAuthoring, CurrentPhase>
    {
    }



}
