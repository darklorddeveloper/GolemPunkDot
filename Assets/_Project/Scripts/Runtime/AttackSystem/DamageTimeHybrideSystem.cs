using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [UpdateBefore(typeof(DamageTextSystem))]
    public partial class DamageTimeHybrideSystem : SystemBase
    {
        private int hash = Shader.PropertyToID("_DamageTime");
        protected override void OnUpdate()
        {
            float time = (float)SystemAPI.Time.ElapsedTime;
            foreach (var (damageHybride, damage, e) in SystemAPI.Query<DamageTimeHybride, Damage>().WithEntityAccess())
            {
                for (int i = 0, length = damageHybride.renderers.Count; i < length; i++)
                {
                    damageHybride.renderers[i].material.SetFloat(hash, time);
                }
                if (EntityManager.HasComponent<PlayHybridAnimation>(e))
                {
                    EntityManager.SetComponentData(e, new PlayHybridAnimation { animationName = "TakeDamage", period = 0.1f });
                    EntityManager.SetComponentEnabled<PlayHybridAnimation>(e, true);
                }
            }
        }
    }
}
