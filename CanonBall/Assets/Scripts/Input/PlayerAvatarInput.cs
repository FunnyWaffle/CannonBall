using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Input
{
    public class PlayerAvatarInput : IInputActionMap
    {
        private readonly InputSystem_Actions.PlayerActions _actions = new();

        public PlayerAvatarInput(InputSystem_Actions.PlayerActions inputActions)
        {
            _actions = inputActions;
            _actions.Interact.performed += context => InteractionActionPerformed?.Invoke();
            _actions.FirstPersonView.performed += OnFirstPersonView;
            _actions.ThirdPersonView.performed += OnPersonThirdView;
            _actions.Back.performed += context => BackActionPerformed?.Invoke();
            _actions.Attack.performed += OnAttack;
            _actions.Jump.performed += OnJumpPerform;
            _actions.Inventory.performed += OnInventoryActionPerformed;
        }

        public InputType Type => InputType.Player;

        public Vector2 Movement => _actions.Move.ReadValue<Vector2>();
        public Vector2 Look => _actions.Look.ReadValue<Vector2>();

        public event Action JumpActionPerformed;
        public event Action AttackActionPerformed;
        public event Action InteractionActionPerformed;
        public event Action<int> ViewModeActionPerformed;
        public event Action BackActionPerformed;
        public event Action InventoryActionPerformed;

        public void Enable()
        {
            _actions.Enable();
        }

        public void Disable()
        {
            _actions.Disable();
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            AttackActionPerformed?.Invoke();
        }

        private void OnInventoryActionPerformed(InputAction.CallbackContext context)
        {
            InventoryActionPerformed?.Invoke();
        }

        private void OnJumpPerform(InputAction.CallbackContext context)
        {
            JumpActionPerformed?.Invoke();
        }

        private void OnPersonThirdView(InputAction.CallbackContext context)
        {
            ViewModeActionPerformed?.Invoke(1);
        }

        private void OnFirstPersonView(InputAction.CallbackContext context)
        {
            ViewModeActionPerformed?.Invoke(0);
        }
    }
}