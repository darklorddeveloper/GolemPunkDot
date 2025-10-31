using UnityEngine;
using Unity.Collections;
using Unity.Entities;
namespace DarkLordGame
{
    public partial class HybridAnimationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            //this is for handy in future not forget
            // var ecb = new EntityCommandBuffer(Allocator.Temp);
            // foreach (var (setup, entity) in SystemAPI.Query<SetupHybridAnimation>().WithEntityAccess()) 
            // {
            //     var anim = setup.targetGameObject.GetComponent<Animator>();
            //     ecb.AddComponent<HybridAnimation>(entity);
            //     ecb.AddComponent<PlayHybridAnimation>(entity);
            //     ecb.SetComponentEnabled<PlayHybridAnimation>(entity, false);
            //     ecb.SetComponentEnabled<SetupHybridAnimation>(entity, false);
            // }
            // ecb.Playback(EntityManager);
            // ecb.Dispose();

            foreach (var (anim, play, entity) in SystemAPI.Query<HybridAnimation, PlayHybridAnimation>().WithEntityAccess())
            {
                if (play.period > 0)
                {
                    anim.animator.CrossFade(play.animationName, play.period);
                }
                else
                {
                    anim.animator.Play(play.animationName);
                }
                EntityManager.SetComponentEnabled<PlayHybridAnimation>(entity, false);
            }

        }
    }
}
