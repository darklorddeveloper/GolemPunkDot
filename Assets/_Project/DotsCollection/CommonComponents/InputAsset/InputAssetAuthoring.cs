using UnityEngine;
using UnityEngine.InputSystem;

namespace DarkLordGame
{
    public class InputAssetAuthoring : ClassAuthorizer<InputAsset>
    {
    }

    public class InputAssetBaker : ClassBaker<InputAssetAuthoring, InputAsset>
    {
    }

    //if use Input action directly then the project will go into infinite compile loop. Unity bug
    [System.Serializable]
    public class InputAsset : ClassComponentData
    {
        public InputActionAsset inputActionAsset;

    }
}
