using System;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Input
{
    public class ShopInput : IInputActionMap
    {
        private readonly InputSystem_Actions.ShopActions _actions;

        public ShopInput(InputSystem_Actions.ShopActions actions)
        {
            _actions = actions;

            _actions.Close.performed += OnClose;
        }

        public InputType Type => InputType.Shop;

        public bool CanBeInStack => false;

        public event Action Closed;

        public void Disable()
        {
            _actions.Disable();
        }

        public void Enable()
        {
            _actions.Enable();
        }

        private void OnClose(InputAction.CallbackContext context)
        {
            Closed?.Invoke();
        }
    }
}
