using System.Collections.Generic;

namespace Assets.Scripts.Input
{
    public class InputSystem
    {
        private readonly Dictionary<InputType, IInputSheme> _shemes = new();

        private readonly InputSystem_Actions _inputActions;

        private IInputSheme _inputSheme;

        public InputSystem(InputSystem_Actions inputActions, params IInputSheme[] inputShemes)
        {
            _inputActions = inputActions;
            _inputActions.Enable();

            foreach (var sheme in inputShemes)
            {
                _shemes[sheme.Type] = sheme;
            }

            EnableSheme(InputType.Gameplay);
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
        Gameplay,
        Placement,
    }
}
