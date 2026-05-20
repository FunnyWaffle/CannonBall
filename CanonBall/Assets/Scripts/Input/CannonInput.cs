using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Input
{
    public class CannonInput : IInputActionMap
    {
        private readonly InputSystem_Actions.CannonActions _actions;

        public CannonInput(InputSystem_Actions.CannonActions actions)
        {
            _actions = actions;

            _actions.Shoot.performed += OnShoot;
        }

        public InputType Type => InputType.Cannon;

        public Vector2 Look => _actions.Look.ReadValue<Vector2>();

        public event Action ShootPerform;

        public void Disable()
        {
            _actions.Disable();
        }

        public void Enable()
        {
            _actions.Enable();
        }

        private void OnShoot(InputAction.CallbackContext context)
        {
            ShootPerform?.Invoke();
        }
    }
}
