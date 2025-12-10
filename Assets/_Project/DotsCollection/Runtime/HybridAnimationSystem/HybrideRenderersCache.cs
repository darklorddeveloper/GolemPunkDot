using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public class HybrideRenderersCache : ClassComponentData
    {
        [Header("Must be add after SetupHybridAnimation")]
        public List<Renderer> renderers = new();
        public override void Init(Entity entity, EntityManager manager)
        {
            base.Init(entity, manager);
            if (manager.HasComponent<SafeCleanupObject>(entity))
            {
                var target = manager.GetComponentData<SafeCleanupObject>(entity);
                var t = target.mainGameObject.GetComponentsInChildren<Renderer>();
                if (t != null)
                    renderers.AddRange(t);
            }

        }
    }
}
