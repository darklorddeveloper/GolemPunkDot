using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public abstract class InteractableObject : ScriptableObject
    {
        public virtual bool hasLongPressInteraction => false;
        public abstract void OnInteract(EntityManager entityManager);

        public abstract void LongPressInteract(EntityManager entityManager);
    }
}
