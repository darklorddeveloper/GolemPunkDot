using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{

    public class GolemEntityAuthoring : ClassAuthorizer<GolemEntity>
    {
    }

    public class GolemEntityBaker :ClassBaker<GolemEntityAuthoring, GolemEntity>
    {
        
    }

    [System.Serializable]
    public class GolemEntity : ClassComponentData
    {
        public Golem golem;

        public override void Init(Entity entity, EntityManager manager)
        {
            base.Init(entity, manager);
            golem = GameObject.Instantiate<Golem>(golem);
            golem.Init();

            var anim = golem.animator;
            manager.AddComponentObject(entity, new TransformSync { targetTransform = golem.transform });
            manager.AddComponentObject(entity, new HybridAnimation { animator = anim });
            manager.AddComponentObject(entity, new PlayHybridAnimation());
            var hybridLocomotion = new HybridLocomotion
            {
                lerpSpeed = golem.lerpLocomotionSpeed
            };
            manager.AddComponentData(entity, hybridLocomotion);
            manager.SetComponentEnabled<PlayHybridAnimation>(entity, false);
        }
    }
}
