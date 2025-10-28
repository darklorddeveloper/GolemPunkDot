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
        private Entity focusingEntity;
        private float timeCount = 0;
        private const float CastInterval = 0.1f;
        private bool initlizedInput = false;
        private bool isPressing = false;
        public InputAction interactAction;
        private double pressedInteractTime;
        private const double interactTimeSteps = 0.2f;
        private const double longInteractTimeSteps = 3.5f;

        private float fadeOutUIPeriod = 0.3f;
        private float fadeOutTimeCount = 0;
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<InputAsset>();
        }

        protected override void OnUpdate()
        {
            UpdateFocus();
            UpdateInteractableUITutorialObject();
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
                        focusingEntity = body.Entity;
                        return;
                    }
                    else
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

        private void UpdateInteractableUITutorialObject()
        {
            if (SystemAPI.ManagedAPI.TryGetSingleton<InteractionUIEntity>(out var interactionUI))
            {
                if (interactionUI.initialized == false) return;
                if (currentFocusingObject == null)
                {
                    if (fadeOutTimeCount <= 0)
                    {
                        //play fadeout animation
                    }
                    fadeOutTimeCount += SystemAPI.Time.DeltaTime;
                    interactionUI.ui.gameObject.SetActive(fadeOutTimeCount < fadeOutUIPeriod);
                    return;
                }
                interactionUI.ui.gameObject.SetActive(true);
                var transform = EntityManager.GetComponentData<LocalToWorld>(focusingEntity);
                var position = transform.Position;

                if (SystemAPI.ManagedAPI.TryGetSingleton<MainCamera>(out var mainCamera))
                {
                    Vector3 screenPos = mainCamera.camera.WorldToScreenPoint(position);
                    interactionUI.ui.rootTransform.position = screenPos;
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
                        currentFocusingObject = null;
                    }
                }
            }
            if (currentFocusingObject == null) return;

            if (interactAction.WasReleasedThisFrame())
            {
                double diff = SystemAPI.Time.ElapsedTime - pressedInteractTime;
                if (diff < interactTimeSteps)
                {
                    currentFocusingObject.OnInteract(EntityManager);
                    currentFocusingObject = null;
                }
                isPressing = false;
            }

        }
    }
}
