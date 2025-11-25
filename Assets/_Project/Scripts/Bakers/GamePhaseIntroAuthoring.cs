using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class GamePhaseIntroAuthoring : EnableStructAuthorizer<GamePhaseIntro>
    {
    }

    public class GamephaseIntroBaker : EnableStructBaker<GamePhaseIntroAuthoring, GamePhaseIntro>
    {

    }


}
