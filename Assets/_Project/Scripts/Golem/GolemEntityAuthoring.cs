using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class GolemEntityAuthoring : ClassAuthorizer<GolemEntity>
    {
    }

    public class GolemEntityBaker : ClassBaker<GolemEntityAuthoring, GolemEntity>
    {

    }

    [System.Serializable]
    public class GolemEntity : ClassComponentData
    {
        public Golem golem;

        private static ComponentTypeSet typeset = new ComponentTypeSet(
            ComponentType.ReadWrite<TransformSync>(),
            ComponentType.ReadWrite<HybridAnimation>(),
            ComponentType.ReadWrite<PlayHybridAnimation>(),
            ComponentType.ReadWrite<HybridLocomotion>(),
            ComponentType.ReadWrite<SafeCleanupObject>()
        );
        public override void Init(Entity entity, EntityManager manager)
        {
            base.Init(entity, manager);
            golem = GameObject.Instantiate<Golem>(golem);
            golem.Init();

            var anim = golem.animator;

            manager.AddComponent(entity, typeset);

            manager.SetComponentData(entity, new TransformSync { targetTransform = golem.transform });
            manager.SetComponentData(entity, new HybridAnimation { animator = anim });
            manager.SetComponentData(entity, new PlayHybridAnimation());
            var hybridLocomotion = new HybridLocomotion
            {
                lerpSpeed = golem.lerpLocomotionSpeed
            };
            manager.SetComponentData(entity, hybridLocomotion);
            manager.SetComponentEnabled<PlayHybridAnimation>(entity, false);
            manager.SetComponentData(entity, new SafeCleanupObject{mainGameObject = golem.gameObject});
        }
    }
}
