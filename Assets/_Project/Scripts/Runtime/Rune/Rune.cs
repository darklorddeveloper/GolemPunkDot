using UnityEngine;
using UnityEngine.Localization;

namespace DarkLordGame
{
    //damage apply once should add into the attackrequest
    public class Rune : ScriptableObject
    {
        public LocalizedString runeName;
        public LocalizedString description;
        public EffectBase effect;
    }
}
