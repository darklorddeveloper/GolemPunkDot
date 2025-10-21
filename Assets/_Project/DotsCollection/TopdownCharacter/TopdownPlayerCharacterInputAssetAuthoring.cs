using UnityEngine;
using UnityEngine.InputSystem;

namespace DarkLordGame
{
    public class TopdownPlayerCharacterInputAssetAuthoring : ClassAuthorizer<TopdownPlayerCharacterInputAsset>
    {
    }

    public class TopdownPlayerCharacterInputAssetBaker : ClassBaker<TopdownPlayerCharacterInputAssetAuthoring, TopdownPlayerCharacterInputAsset>
    {
    }

    [System.Serializable]
    public class TopdownPlayerCharacterInputAsset : ClassComponentData
    {
        public InputActionAsset inputActionAsset;
        
    }
}
