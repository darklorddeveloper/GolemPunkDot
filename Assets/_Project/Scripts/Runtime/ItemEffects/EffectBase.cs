using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Localization;

namespace DarkLordGame
{
    public abstract class EffectBase : ScriptableObject
    {
        public LocalizedString effectName;
        public LocalizedString effectDetails;
        public EffectTiming effectTiming = EffectTiming.ActivePrimary;
        public List<FusionTagValue> fusionTagValues = new();
        public abstract void OnActivate(Entity activator, EntityManager entityManager);

    }
}
