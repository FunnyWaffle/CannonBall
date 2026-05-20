using System;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Input
{
    public class PlacementInput : IInputSheme
    {
        private readonly InputSystem_Actions.PlacementActions _actions;

        public PlacementInput(InputSystem_Actions.PlacementActions actions)
        {
            _actions = actions;

            _actions.Place.performed += OnPlace;
        }

        public InputType Type => InputType.Placement;

        public event Action PlacePerformed;

        public void Disable()
        {
            _actions.Disable();
        }

        public void Enable()
        {
            _actions.Enable();
        }

        private void OnPlace(InputAction.CallbackContext context)
        {
            PlacePerformed?.Invoke();
        }
    }
}
