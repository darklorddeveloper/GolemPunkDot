using Unity.Entities;

using Unity.Physics;
using Unity.Transforms;
using UnityEngine.InputSystem;
using UnityEngine;

namespace DarkLordGame
{
    public partial class InteractionSystem : SystemBase
    {
        private InteractableObject currentFocusingObject;
        private float timeCount = 0;
        private const float CastInterval = 0.1f;
        private bool initlizedInput = false;
        private bool isPressing = false;
        public InputAction interactAction;
        private double pressedInteractTime;
        private const double interactTimeSteps = 0.1f;
        private const double longInteractTimeSteps = 3.5f;
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<InputAsset>();
        }

        protected override void OnUpdate()
        {
            UpdateFocus();
            UpdateInteraction();
        }

        private void UpdateFocus()
        {
            timeCount += SystemAPI.Time.DeltaTime;
            if (timeCount < CastInterval)
            {
                return;
            }
            timeCount = 0;

            var physics = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            var cw = physics.CollisionWorld; // a value type, safe to capture

            foreach (var (player, interactor, transform) in SystemAPI.Query<PlayerComponent, Interactor, LocalTransform>())
            {
                // transform.Position
                ColliderCastHit hit;
                bool hitted = cw.SphereCast(transform.Position, interactor.radius, new Unity.Mathematics.float3(0, 1, 0), interactor.radius, out hit,
                new CollisionFilter
                {
                    BelongsTo = 1,
                    CollidesWith = interactor.interactLayerBit,
                });
                if (hitted)
                {
                    var body = cw.Bodies[hit.RigidBodyIndex];
                    if (EntityManager.HasComponent<InteractableEntity>(body.Entity))
                    {
                        currentFocusingObject = EntityManager.GetComponentObject<InteractableEntity>(body.Entity).interactableObject;
                        return;
                    } else
                    {
                        currentFocusingObject = null;
                    }
                }
                else
                {
                    currentFocusingObject = null;
                }
            }
        }

        private void UpdateInteraction()
        {
            if (currentFocusingObject == null) return;
            var inputAsset = SystemAPI.ManagedAPI.GetSingleton<InputAsset>();
            if (inputAsset.initialized == false) return;

            if (initlizedInput == false)
            {
                initlizedInput = true;
                var targetInputAsset = inputAsset.inputActionAsset;

                var map = targetInputAsset.FindActionMap("Player", throwIfNotFound: true);
                interactAction = map["Interact"];
                interactAction.Enable();
            }
            if (interactAction.WasPerformedThisFrame())
            {
                pressedInteractTime = SystemAPI.Time.ElapsedTime;
                isPressing = true;
            }

            if (interactAction.IsPressed())
            {
                if (isPressing)
                {
                    double diff = SystemAPI.Time.ElapsedTime - pressedInteractTime;
                    if (diff >= longInteractTimeSteps)
                    {
                        isPressing = false;
                        currentFocusingObject.LongPressInteract(EntityManager);
                    }
                }
            }

            if (interactAction.WasReleasedThisFrame())
            {
                double diff = SystemAPI.Time.ElapsedTime - pressedInteractTime;
                if (diff < interactTimeSteps)
                {
                    currentFocusingObject.OnInteract(EntityManager);
                }
                isPressing = false;
            }

        }
    }
}
