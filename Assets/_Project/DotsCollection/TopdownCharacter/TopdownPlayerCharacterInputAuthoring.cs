using UnityEngine;
using UnityEngine.InputSystem;

namespace DarkLordGame
{
    public class TopdownPlayerCharacterInputAuthoring : ClassAuthorizer<TopdownPlayerCharacterInput>
    {
    }

    public class TopdownPlayerCharacterInputBaker : ClassBaker<TopdownPlayerCharacterInputAuthoring, TopdownPlayerCharacterInput>
    {
    }

    [System.Serializable]
    public class TopdownPlayerCharacterInput : ClassComponentData
    {
        public InputActionAsset inputActionAsset;
        
    }
}
