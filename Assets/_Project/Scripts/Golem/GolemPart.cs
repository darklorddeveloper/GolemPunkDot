using UnityEngine;
using UnityEngine.Localization;

namespace DarkLordGame
{
    public class GolemPart : ScriptableObject
    {
        public LocalizedString partName;
        // public LocalizedString partDescription; <=
        public GolemPartType partType;
        public Mesh mesh;
        public Material[] materials;
        [System.NonSerialized] public GameObject runtimSkinnedObject;
        [System.NonSerialized] public bool isInstance;
        public virtual string GetLocalizedDescription()
        {
            //do something
            return "";
        }

        //effects related
    }
}
