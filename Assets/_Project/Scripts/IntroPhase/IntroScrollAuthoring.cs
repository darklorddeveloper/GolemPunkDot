using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class IntroScrollAuthoring : ClassAuthorizer<IntroScroll>
    {

    }

    public class IntroScrollBaker : ClassBaker<IntroScrollAuthoring, IntroScroll>
    {

    }

    [System.Serializable]
    public class IntroScroll : ClassComponentData
    {
        public GameObject scrollObject;
        [System.NonSerialized] public Animator scrollAnimator;
        public override void Init()
        {
            base.Init();

            scrollObject = GameObject.Instantiate(scrollObject);
            scrollAnimator = scrollObject.GetComponent<Animator>();
        }
    }
}
