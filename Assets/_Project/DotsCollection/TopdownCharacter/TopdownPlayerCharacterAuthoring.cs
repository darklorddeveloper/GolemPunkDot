using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class TopdownPlayerCharacterAuthoring : StructAuthorizer<TopdownPlayerCharacter, TopdownCharacterInput, TopdownCharacterMovement>

    {

    }
    
    public class TopdownPlayerCharacterBaker : StructBaker<TopdownPlayerCharacterAuthoring, TopdownPlayerCharacter, TopdownCharacterInput, TopdownCharacterMovement>
    {
        
    }
    [System.Serializable]
    public struct TopdownPlayerCharacter : IComponentData{}
}
