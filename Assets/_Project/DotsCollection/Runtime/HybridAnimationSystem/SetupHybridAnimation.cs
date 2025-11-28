using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public class SetupHybridAnimation : ClassComponentData
    {
        public GameObject prefab;

        public override void Init(Entity entity, EntityManager manager)
        {
            base.Init(entity, manager);
            var obj = GameObject.Instantiate(prefab);
            var anim = obj.GetComponentInChildren<Animator>();
            manager.SetComponentData(entity, new TransformSync { targetTransform = obj.transform });
            manager.SetComponentData(entity, new HybridAnimation { animator = anim });
            manager.SetComponentEnabled<PlayHybridAnimation>(entity, false);
            manager.AddComponentObject(entity, new SafeCleanupObject { mainGameObject = obj.gameObject });

        }
    }
}
