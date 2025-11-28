using UnityEngine;
using Unity.Entities;

namespace DarkLordGame
{
    [System.Serializable]
    public class GolemEntity : ClassComponentData
    {
        [System.NonSerialized]public Golem golem;

        public override void Init(Entity entity, EntityManager manager)
        {
            base.Init(entity, manager);
            var safeCleanup = manager.GetComponentObject<SafeCleanupObject>(entity);
            golem = safeCleanup.mainGameObject.GetComponent<Golem>();
            golem.Init();
        }
    }
}
