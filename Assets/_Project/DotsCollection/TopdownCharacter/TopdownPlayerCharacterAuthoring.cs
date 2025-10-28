using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class TopdownPlayerCharacterAuthoring : StructAuthorizer<PlayerComponent, TopdownCharacterInput, TopdownCharacterMovement>

    {

    }
    
    public class TopdownPlayerCharacterBaker : StructBaker<TopdownPlayerCharacterAuthoring, PlayerComponent, TopdownCharacterInput, TopdownCharacterMovement>
    {
        
    }
}
