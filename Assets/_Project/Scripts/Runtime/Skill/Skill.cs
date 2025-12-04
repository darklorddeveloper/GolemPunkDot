using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Localization;

namespace DarkLordGame
{
    public class Skill : ScriptableObject
    {
        [SerializeField] public LocalizedString skillName;
        [SerializeField] public LocalizedString skillDescription;
        public Sprite icon;
        [System.NonSerialized] public bool isInstance;

        [Header("Actions")]
        public List<GolemActionData> actionDatas = new List<GolemActionData> { new GolemActionData() };
        public EffectBase preActivateEffect;
        public EffectBase postActivateEffect;

        [Header("cooldown")]
        public float cooldownTime = 0.1f;
        public float cooldownTimeCount;
    }
}
