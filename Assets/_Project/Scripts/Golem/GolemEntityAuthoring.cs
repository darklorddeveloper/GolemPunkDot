using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{

    public class GolemEntityAuthoring : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
    public class GolemEntity : ClassComponentData
    {
        public Golem golem;

        public override void Init(Entity entity, EntityManager manager)
        {
            base.Init(entity, manager);
            golem = GameObject.Instantiate<Golem>(golem);
            golem.Init();
        }
    }
}
