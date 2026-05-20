using System;
using UnityEngine;

namespace Assets.Scripts.Input
{
    public class GameplayInput : IInputSheme
    {
        private readonly InputSystem_Actions.PlayerActions _actions = new();

        public Vector2 Movement => _actions.Move.ReadValue<Vector2>();
        public Vector2 Look => _actions.Look.ReadValue<Vector2>();

        public event Action JumpActionPerformed;
        public event Action AttackActionPerformed;
        public event Action InteractionActionPerformed;
        public event Action<int> ViewModeActionPerformed;
        public event Action BackActionPerformed;
        public event Action InventoryActionPerformed;

        public GameplayInput(InputSystem_Actions.PlayerActions inputActions)
        {
            _actions = inputActions;

            _actions.Jump.performed += context => JumpActionPerformed?.Invoke();
            _actions.Attack.performed += context => AttackActionPerformed?.Invoke();
            _actions.Interact.performed += context => InteractionActionPerformed?.Invoke();
            _actions.FirstPersonView.performed += context => ViewModeActionPerformed?.Invoke(0);
            _actions.ThirdPersonView.performed += context => ViewModeActionPerformed?.Invoke(1);
            _actions.Back.performed += context => BackActionPerformed?.Invoke();
            _actions.Inventory.performed += context => InventoryActionPerformed?.Invoke();
        }

        public InputType Type => InputType.Gameplay;

        public void Enable()
        {
            _actions.Enable();
        }

        public void Disable()
        {
            _actions.Disable();
        }
    }
}