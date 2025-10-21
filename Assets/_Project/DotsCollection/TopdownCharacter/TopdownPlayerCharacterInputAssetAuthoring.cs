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

    //if use Input action directly then the project will go into infinite compile loop. Unity bug
    [System.Serializable]
    public class TopdownPlayerCharacterInputAsset : ClassComponentData
    {
        public InputActionAsset inputActionAsset;

    }
}
