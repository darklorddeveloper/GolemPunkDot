using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class PlayerFlagAuthoring : StructAuthorizer<PlayerFlag>
    {
        
    }
    
    public class PlayerFlagBaker : StructBaker<PlayerFlagAuthoring, PlayerFlag>
    {
    }
    public struct PlayerFlag: IComponentData
    {
        
    }
}
