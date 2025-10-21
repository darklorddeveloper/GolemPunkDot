using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class TopdownPlayerCharacterAuthoring : StructAuthorizer<TopdownPlayerCharacter, TopdownCharacterInput>

    {

    }
    
    public class TopdownPlayerCharacterBaker : StructBaker<TopdownPlayerCharacterAuthoring, TopdownPlayerCharacter, TopdownCharacterInput>
    {
        
    }
    [System.Serializable]
    public struct TopdownPlayerCharacter : IComponentData{}
}
