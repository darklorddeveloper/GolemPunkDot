using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using Unity.Transforms;
namespace DarkLordGame
{
    public partial class TopdownPlayerInputSystem : SystemBase
    {
        private bool initialized = false;
        public InputAction moveAction;
        public InputAction lookAtAction;
        public InputAction dashAction;
        public InputAction primaryAction, secondaryAction;
        public InputAction skill1Action, skill2Action, skill3Action, skill4Action;

        private TopdownCharacterInput currentInput;
        private Plane plane;
        private Ray ray;
        private bool isUsingMouseKeyboard = false;

        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<InputAsset>();
            plane = new Plane(Vector3.up, Vector3.zero);
        }

        bool UsingGamepadThisFrame() => Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame;
        bool UsingKeyboardMouseThisFrame() =>
            (Keyboard.current?.wasUpdatedThisFrame ?? false) || (Mouse.current?.wasUpdatedThisFrame ?? false);

        private void CheckUsingDevice()
        {
            if (isUsingMouseKeyboard)
            {
                if (UsingGamepadThisFrame())
                {
                    isUsingMouseKeyboard = false;
                }
            }
            else
            {
                isUsingMouseKeyboard = UsingKeyboardMouseThisFrame();
            }

        }

        protected override void OnUpdate()
        {
            if (SystemAPI.ManagedAPI.TryGetSingleton<InputAsset>(out var input) == false)
            {
                return;
            }
            if (input.initialized == false)
            {
                return;
            }
            if (initialized == false)
            {
                initialized = true;
                var targetInputAsset = input.inputActionAsset;
                var map = targetInputAsset.FindActionMap("Player", throwIfNotFound: true);
                moveAction = map["Move"];
                lookAtAction = map["Look"];
                primaryAction = map["Primary"];
                secondaryAction = map["Secondary"];
                dashAction = map["Dash"];
                skill1Action = map["Skill1"];
                skill2Action = map["Skill2"];
                skill3Action = map["Skill3"];
                skill4Action = map["Skill4"];

                moveAction.Enable();
                lookAtAction.Enable();
                primaryAction.Enable();
                secondaryAction.Enable();
                dashAction.Enable();
                skill1Action.Enable();
                skill2Action.Enable();
                skill3Action.Enable();
                skill4Action.Enable();
                //do all the cache
            }

            CheckUsingDevice();

            currentInput.primaryAction = primaryAction.WasPerformedThisFrame();
            currentInput.secondaryAction = secondaryAction.WasPerformedThisFrame();
            currentInput.dashAction = dashAction.WasPerformedThisFrame();
            currentInput.skill1 = skill1Action.WasPerformedThisFrame();
            currentInput.skill2 = skill2Action.WasPerformedThisFrame();
            currentInput.skill3 = skill3Action.WasPerformedThisFrame();
            currentInput.skill4 = skill4Action.WasPerformedThisFrame();

            currentInput.isHoldingPrimaryAction = primaryAction.IsPressed();
            currentInput.isHoldingSecondaryAction = secondaryAction.IsPressed();
            currentInput.isHoldingDashAction = dashAction.IsPressed();
            currentInput.isHoldingSkill1 = skill1Action.IsPressed();
            currentInput.isHoldingSkill2 = skill2Action.IsPressed();
            currentInput.isHoldingSkill3 = skill3Action.IsPressed();
            currentInput.isHoldingSkill4 = skill4Action.IsPressed();

            var move = moveAction.ReadValue<Vector2>();
            currentInput.movement = new float3(move.x, 0, move.y);
            MainCamera mainCam = null;
            if (isUsingMouseKeyboard)
            {
                SystemAPI.ManagedAPI.TryGetSingleton(out mainCam);
                if (mainCam != null)
                {
                    ray = mainCam.camera.ScreenPointToRay(Mouse.current.position.value);
                    if (plane.Raycast(ray, out float center))
                    {
                        var point = ray.GetPoint(center);
                        currentInput.lookAtTargetPoint = point;
                    }
                }
            }
            float3 controllerDirection = float3.zero;
            if (isUsingMouseKeyboard == false)
            {
                var diff = lookAtAction.ReadValue<Vector2>();
                controllerDirection = math.lengthsq(diff) > 0.01f ? new float3(diff.x, 0, diff.y) : currentInput.movement;
            }

            foreach (var (inputCharacter, character, transform, e) in SystemAPI.Query<RefRW<PlayerComponent>, TopdownCharacterInput, LocalTransform>().WithEntityAccess())
            {
                if (isUsingMouseKeyboard == false)
                {
                    currentInput.lookAtTargetPoint = transform.Position + controllerDirection;
                }
                inputCharacter.ValueRW.playerPosition = transform.Position;
                EntityManager.SetComponentData(e, currentInput);
            }
        }


    }
}
