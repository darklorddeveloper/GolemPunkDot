using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    public partial class HybridLocomotionSystem : SystemBase
    {
        public static int propertyX = Animator.StringToHash("Horizontal");
        public static int propertyY = Animator.StringToHash("Vertical");
        protected override void OnUpdate()
        {
            foreach (var (anim, hybridTransform) in SystemAPI.Query<HybridAnimation, TransformSync>().WithAll<HybridLocomotion>())
            {
                if (hybridTransform.deltaPosition.sqrMagnitude > 0.1f)
                {
                    var norm = hybridTransform.deltaPosition.normalized;
                    var x = Vector3.Dot(hybridTransform.targetTransform.right, norm);
                    var y = Vector3.Dot(hybridTransform.targetTransform.forward, norm);
                    anim.animator.SetFloat(propertyX, x);
                    anim.animator.SetFloat(propertyY, y);
                }
                else
                {
                    anim.animator.SetFloat(propertyX, 0);
                    anim.animator.SetFloat(propertyY, 0);
                }
            }
        }
    }
}
