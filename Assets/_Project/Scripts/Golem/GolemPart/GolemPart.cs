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
        public class ActionData
        {
            public string startAnimationName = "Attack1";
            public float activateDelayed = 0.06f;
            public float totalPeriod = 0.4f;
            public float movementSpeedRate = 1.0f;
            public bool isHoldable = false;
            public float maxHoldPeriod = 1.5f;
            public string releaseAnimationName = "Release";
        }

        public List<ActionData> actionDatas = new List<ActionData> { new ActionData() };

        public void Init()
        {
            for (int i = 0, length = effects.Count; i < length; i++)
            {
                effects[i] = ScriptableObject.Instantiate(effects[i]);
            }
        }
    }
}
