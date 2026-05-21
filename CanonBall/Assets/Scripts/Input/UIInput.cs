using System;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Input
{
    public class UIInput : IInputActionMap
    {
        private readonly InputSystem_Actions.UIActions _actions;

        public UIInput(InputSystem_Actions.UIActions actions)
        {
            _actions = actions;

            _actions.Cancel.performed += OnCancel;
        }

        public InputType Type => InputType.UI;

        public event Action CancelPefromed;

        public void Disable()
        {
            _actions.Disable();
        }

        public void Enable()
        {
            _actions.Enable();
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            CancelPefromed?.Invoke();
        }
    }
}
