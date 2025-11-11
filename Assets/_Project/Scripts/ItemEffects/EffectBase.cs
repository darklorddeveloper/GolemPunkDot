using Unity.Entities;
using UnityEngine;
using UnityEngine.Localization;

namespace DarkLordGame
{
    public abstract class EffectBase : ScriptableObject
    {
        public LocalizedString effectName;
        public LocalizedString effectDetails;
        public EffectTiming effectTiming;

        public abstract void Activate(Entity activator, EntityManager entityManager);//things that needed
    }
}
