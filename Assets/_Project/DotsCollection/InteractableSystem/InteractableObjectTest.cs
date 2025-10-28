using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [CreateAssetMenu(fileName = "Interactable Test", menuName = "Interactable/Test")]
    public class InteractableObjectTest : InteractableObject
    {
        public override bool hasLongPressInteraction => true;
        public override void OnInteract(EntityManager entityManager)
        {
            Debug.Log("on interact");
        }

        public override void LongPressInteract(EntityManager entityManager)
        {
            Debug.Log("on long interact");
        }
    }
}
