using System.Collections.Generic;

namespace Assets.Scripts.Input
{
    public class InputSystem
    {
        private readonly Dictionary<InputType, IInputActionMap> _shemes = new();
        private readonly Stack<IInputActionMap> _lastInputActionMap = new();

        private readonly InputSystem_Actions _inputActions;

        private IInputActionMap _inputActionMap;

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
            DisableActualMap();
            EnableMap(inputType);
        }

        public void SwitchToLast()
        {
            var last = _lastInputActionMap.Pop();

            DisableActualMap();

            last.Enable();
            _inputActionMap = last;
        }

        public void DisableCurrent()
        {
            DisableActualMap();
            _inputActionMap = null;
        }

        private void DisableActualMap()
        {
            if (_inputActionMap == null)
                return;

            _inputActionMap.Disable();

            if (_inputActionMap.CanBeInStack)
                _lastInputActionMap.Push(_inputActionMap);
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
        Inventory,
        Shop,
    }
}
