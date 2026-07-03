using System;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Input
{
    public class InventoryInput : IInputActionMap
    {
        private readonly InputSystem_Actions.InventoryActions _actions;

        public InventoryInput(InputSystem_Actions.InventoryActions actions)
        {
            _actions = actions;

            _actions.Close.performed += OnClosed;
        }

        public InputType Type => InputType.Inventory;
        public bool CanBeInQueue => false;

        public event Action ClosePefromed;

        public void Disable()
        {
            _actions.Disable();
        }

        public void Enable()
        {
            _actions.Enable();
        }

        private void OnClosed(InputAction.CallbackContext context)
        {
            ClosePefromed?.Invoke();
        }
    }
}
