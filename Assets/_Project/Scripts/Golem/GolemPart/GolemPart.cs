using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
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

        public List<EffectBase> effects = new();

        public void Init()
        {
            for (int i = 0, length = effects.Count; i < length; i++)
            {
                effects[i] = ScriptableObject.Instantiate(effects[i]);
            }
        }

        // public list
        public virtual string GetLocalizedDescription()
        {
            //do something
            return "";
        }

        //effects related
        
        public virtual IEnumerator OnManualActivate(Entity activator, EntityManager manager)
        {
            //do something with timing.
            yield break;
        }
    }
}
