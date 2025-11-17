using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public abstract class GolemPartComboSequence : GolemPart
    {
        [System.Serializable]
        public class ComboData
        {
            public float totalPeriod = 0.3f;
            public float activateDelayed = 0.1f;
            public string animationName;
        }

        public List<ComboData> combos = new List<ComboData>();
        private int currentIndex = 0;
      
    }
}

