using Unity.Entities;
using UnityEngine;
using UnityEngine.Localization;

namespace DarkLordGame
{
    public abstract class EffectBase : ScriptableObject
    {
        public LocalizedString effectName;
        public LocalizedString effectDetails;
        public EffectTiming effectTiming = EffectTiming.ManualActivate;
        public abstract void OnActivate(Entity activator, EntityManager entityManager);
    }
}
