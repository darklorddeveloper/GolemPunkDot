using UnityEngine;
using UnityEngine.Localization;

namespace DarkLordGame
{
    [CreateAssetMenu(fileName = "DifficultyNormal", menuName = "Difficulty")]
    public class DifficultyData : ScriptableObject
    {

        public LocalizedString difficultyName;
        public LocalizedString descriptions;
        [GUID] public string difficultyPrefab;

        public float distanceFromCenter;
    }
}
