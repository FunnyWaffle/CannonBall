using System.Collections.Generic;

namespace Assets.Scripts.Input
{
    public class InputSystem
    {
        private readonly Dictionary<InputType, IInputActionMap> _shemes = new();

        private readonly InputSystem_Actions _inputActions;

        private IInputActionMap _inputActionMap;
        private IInputActionMap _lastInputActionMap;

        public InputSystem(InputSystem_Actions inputActions, params IInputActionMap[] inputActionMaps)
        {
            _inputActions = inputActions;
            _inputActions.Enable();

            foreach (var map in inputActionMaps)
            {
                map.Disable();
                _shemes[map.Type] = map;
            }

            EnableMap(InputType.Player);
        }

        public void SwitchTo(InputType inputType)
        {
            _lastInputActionMap = _inputActionMap;
            _inputActionMap.Disable();
            EnableMap(inputType);
        }

        public void SwitchToLast()
        {
            var last = _lastInputActionMap;

            _inputActionMap.Disable();
            _lastInputActionMap = _inputActionMap;

            last.Enable();
            _inputActionMap = last;
        }

        private void EnableMap(InputType inputType)
        {
            _inputActionMap = _shemes[inputType];
            _inputActionMap.Enable();
        }
    }

    public enum InputType
    {
        Player,
        Cannon,
        Placement,
        UI,
    }
}
