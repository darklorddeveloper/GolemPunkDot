using Unity.Entities;
using UnityEngine;


namespace DarkLordGame
{
    public class GameStatAuthoring : StructAuthorizer<GameStat>
    {
        
    }
    
    public class GameStatBaker : StructBaker<GameStatAuthoring, GameStat>
    {
    }

    [System.Serializable]
    public struct GameStat : IComponentData
    {
    }
}