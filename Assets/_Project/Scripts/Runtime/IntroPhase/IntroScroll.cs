using UnityEngine;
using Unity.Entities;

namespace DarkLordGame
{
  [System.Serializable]
    public class IntroScroll : ClassComponentData
    {
        public GameObject scrollObject;
        [System.NonSerialized] public Animator scrollAnimator;
        // story assets
        // next pages.
        // skip button
        public override void Init(Entity entity, EntityManager manager)
        {
            base.Init(entity, manager);
            scrollObject = GameObject.Instantiate(scrollObject);
            scrollAnimator = scrollObject.GetComponent<Animator>();

        }
    }
}
