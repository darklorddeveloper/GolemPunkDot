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
            float deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var (anim, hybridTransform, locomotion) in SystemAPI.Query<HybridAnimation, TransformSync, RefRW<HybridLocomotion>>())
            {
                float x = 0, y = 0;
                if (hybridTransform.deltaPosition.sqrMagnitude > 0.001f)
                {
                    var norm = hybridTransform.deltaPosition.normalized;
                    x = Vector3.Dot(hybridTransform.targetTransform.right, norm);
                    y = Vector3.Dot(hybridTransform.targetTransform.forward, norm);
                }
                locomotion.ValueRW.x = Mathf.Lerp(locomotion.ValueRO.x, x, locomotion.ValueRO.lerpSpeed * deltaTime);
                locomotion.ValueRW.y = Mathf.Lerp(locomotion.ValueRO.y, y, locomotion.ValueRO.lerpSpeed * deltaTime);
                anim.animator.SetFloat(propertyX, locomotion.ValueRO.x);
                anim.animator.SetFloat(propertyY, locomotion.ValueRO.y);
            }
        }
    }
}
