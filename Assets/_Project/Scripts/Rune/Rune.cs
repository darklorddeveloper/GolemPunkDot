using UnityEngine;
using UnityEngine.Localization;

namespace DarkLordGame
{
    public class Rune : ScriptableObject
    {
        public LocalizedString runeName;
        public LocalizedString description;
        public EffectBase effect;
    }
}
