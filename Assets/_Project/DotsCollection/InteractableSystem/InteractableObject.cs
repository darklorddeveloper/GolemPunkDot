using System;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public abstract class InteractableObject : ScriptableObject
    {
        public bool hasLongPressInteraction;
        public abstract void OnInteract(EntityManager entityManager);

        public abstract void LongPressInteract(EntityManager entityManager);
    }
}
