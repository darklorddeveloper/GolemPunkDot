using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public class GolemActionData
    {
        public string startAnimationName = "Attack1";
        public float crossFade = 0.0f;
        public float activateDelayed = 0.06f;
        public float totalPeriod = 0.4f;
        
        public bool isHoldable = false;
        public string releaseAnimationName = "Release";

        public AnimationCurve movement = AnimationCurve.Linear(0, 0, 1.0f, 0);
    }
}
