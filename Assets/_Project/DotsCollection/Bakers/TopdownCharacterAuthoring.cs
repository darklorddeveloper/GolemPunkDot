using UnityEngine;

namespace DarkLordGame
{
    public class TopdownCharacterAuthoring : StructAuthorizer<TopdownCharacterInput, TopdownCharacterMovement, MovementSpeed>
    {
    }

    public class TopdownCharacterBaker : StructBaker<TopdownCharacterAuthoring,TopdownCharacterInput, TopdownCharacterMovement, MovementSpeed>
    {
        
    }
}
