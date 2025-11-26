using UnityEngine;
using Unity.Entities;

namespace DarkLordGame
{
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

        private static ComponentTypeSet typeset2 = new ComponentTypeSet(
            ComponentType.ReadWrite<AttackRequestData>()
        );
        public override void Init(Entity entity, EntityManager manager)
        {
            base.Init(entity, manager);
            golem = GameObject.Instantiate<Golem>(golem);
            golem.Init();

            var anim = golem.animator;

            manager.AddComponent(entity, typeset);
            manager.AddComponent(entity, typeset2);

            manager.SetComponentData(entity, new TransformSync { targetTransform = golem.transform });
            manager.SetComponentData(entity, new HybridAnimation { animator = anim });
            manager.SetComponentData(entity, new PlayHybridAnimation());
            var hybridLocomotion = new HybridLocomotion
            {
                lerpSpeed = golem.lerpLocomotionSpeed
            };
            manager.SetComponentData(entity, hybridLocomotion);
            manager.SetComponentEnabled<PlayHybridAnimation>(entity, false);
            manager.SetComponentData(entity, new SafeCleanupObject { mainGameObject = golem.gameObject });

            manager.SetComponentEnabled<AttackRequestData>(entity, false);
        }
    }
}
