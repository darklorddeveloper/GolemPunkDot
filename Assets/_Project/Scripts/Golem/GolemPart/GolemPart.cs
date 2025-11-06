using UnityEngine;
using UnityEngine.Localization;

namespace DarkLordGame
{
    [CreateAssetMenu(fileName = "GolemPart", menuName = "GolemPart/DefaultPart")]
    public class GolemPart : ScriptableObject
    {
        public LocalizedString partName;
        // public LocalizedString partDescription; <=
        public GolemPartType partType;
        public Mesh mesh;
        public Material[] materials;
        [Header("Special")]
        public bool isUsingAttachPoint;
        public GolemAttachPoint attachPoint;
        
        [System.NonSerialized] public GameObject runtimeGameObject;
        [System.NonSerialized] public bool isInstance;
        
        // public list
        public virtual string GetLocalizedDescription()
        {
            //do something
            return "";
        }

        //effects related
    }
}
