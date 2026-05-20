using System.Collections.Generic;

namespace Assets.Scripts.Input
{
    public class InputSystem
    {
        private readonly Dictionary<InputType, IInputActionMap> _shemes = new();

        private readonly InputSystem_Actions _inputActions;

        private IInputActionMap _inputSheme;

        public InputSystem(InputSystem_Actions inputActions, params IInputActionMap[] inputShemes)
        {
            _inputActions = inputActions;
            _inputActions.Enable();

            foreach (var sheme in inputShemes)
            {
                sheme.Disable();
                _shemes[sheme.Type] = sheme;
            }

            EnableSheme(InputType.Player);
        }

        public void SwitchTo(InputType inputType)
        {
            _inputSheme.Disable();
            EnableSheme(inputType);
        }

        private void EnableSheme(InputType inputType)
        {
            _inputSheme = _shemes[inputType];
            _inputSheme.Enable();
        }
    }

    public enum InputType
    {
        Player,
        Cannon,
        Placement,
    }
}
