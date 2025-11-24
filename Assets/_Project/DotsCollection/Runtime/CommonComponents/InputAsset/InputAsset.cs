using UnityEngine;
using UnityEngine.InputSystem;

namespace DarkLordGame
{

    //if use Input action directly then the project will go into infinite compile loop. Unity bug
    [System.Serializable]
    public class InputAsset : ClassComponentData
    {
        public InputActionAsset inputActionAsset;

    }
}
