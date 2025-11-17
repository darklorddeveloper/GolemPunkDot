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

        [Header("Runes")]
        public int runeSlots = 3;
        public List<Rune> runes = new();

        [Header("Actions")]
        public int currentActionIndex;
        public List<GolemActionData> actionDatas = new List<GolemActionData> { new GolemActionData() };

        [Header("cooldown")]
        public float cooldownTime = 0.1f;
        public float cooldownTimeCount;


        public void Init()
        {
            for (int i = 0, length = effects.Count; i < length; i++)
            {
                effects[i] = ScriptableObject.Instantiate(effects[i]);
            }
        }
    }
}
