using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class GamePhaseAuthoring : EnableStructAuthorizer<GamePhase>
    {
        
    }

    public class GamePhaseBaker : EnableStructBaker<GamePhaseAuthoring, GamePhase>
    {
    }
 


}
