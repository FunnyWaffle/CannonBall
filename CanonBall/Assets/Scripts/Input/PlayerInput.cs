using System;
using UnityEngine;

namespace Assets.Scripts.Input
{
    public class PlayerInput
    {
        private readonly InputSystem_Actions _actions = new();

        public Vector2 Movement => _actions.Player.Move.ReadValue<Vector2>();
        public Vector2 Look => _actions.Player.Look.ReadValue<Vector2>();

        public event Action JumpActionPerformed;
        public event Action AttackActionPerformed;
        public event Action InteractionActionPerformed;
        public event Action<int> ViewModeActionPerformed;
        public event Action BackActionPerformed;
        public event Action InventoryActionPerformed;

        public PlayerInput()
        {
            _actions.Enable();

            _actions.Player.Jump.performed += context => JumpActionPerformed?.Invoke();
            _actions.Player.Attack.performed += context => AttackActionPerformed?.Invoke();
            _actions.Player.Interact.performed += context => InteractionActionPerformed?.Invoke();
            _actions.Player.FirstPersonView.performed += context => ViewModeActionPerformed?.Invoke(0);
            _actions.Player.ThirdPersonView.performed += context => ViewModeActionPerformed?.Invoke(1);
            _actions.Player.Back.performed += context => BackActionPerformed?.Invoke();
            _actions.Player.Inventory.performed += context => InventoryActionPerformed?.Invoke();
        }
    }
}