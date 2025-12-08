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
        [System.NonSerialized] public GolemPart original;
        public List<EffectBase> effects = new();

        [Header("Runes")]
        public int runeSlots = 3;
        public List<Rune> runes = new();

        [Header("Actions")]
        public List<GolemActionData> actionDatas = new List<GolemActionData> { new GolemActionData() };

        [Header("cooldown")]
        public float cooldownTime = 0.1f;
        public float cooldownTimeCount;

        [Header("Tags for fuse")]
        public List<FusionTagValue> partTagValues = new();
        [Header("Linked part")]
        public List<GolemPart> linkedPart = new();

        public void Init()
        {
            for (int i = 0, length = effects.Count; i < length; i++)
            {
                effects[i] = ScriptableObject.Instantiate(effects[i]);
            }
        }

        public void ExtractFusionScores(float[] scores)
        {
            for (int i = 0, length = effects.Count; i < length; i++)
            {
                for (int j = 0, num = effects[i].fusionTagValues.Count; j < num; j++)
                {
                    var tagValue = effects[i].fusionTagValues[j];
                    scores[(int)tagValue.tagType] += tagValue.value;
                }
            }

            for (int i = 0, length = runes.Count; i < length; i++)
            {
                var fx = runes[i].effect;
                for (int j = 0, num = fx.fusionTagValues.Count; j < num; j++)
                {
                    var tagValue = fx.fusionTagValues[j];
                    scores[(int)tagValue.tagType] += tagValue.value;
                }
            }
        }
    }
}
