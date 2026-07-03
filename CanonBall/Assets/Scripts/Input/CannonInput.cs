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
            _actions.Exit.performed += OnExit;
            _actions.FirstPersonView.performed += OnFirstPersonView;
            _actions.ThirdPersonView.performed += OnPersonThirdView;
        }

        public InputType Type => InputType.Cannon;

        public Vector2 Look => _actions.Look.ReadValue<Vector2>();

        public event Action ShootPerform;
        public event Action ExitPerform;
        public event Action<int> ViewModeActionPerformed;

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

        private void OnExit(InputAction.CallbackContext context)
        {
            ExitPerform?.Invoke();
        }

        private void OnPersonThirdView(InputAction.CallbackContext context)
        {
            ViewModeActionPerformed?.Invoke(1);
        }

        private void OnFirstPersonView(InputAction.CallbackContext context)
        {
            ViewModeActionPerformed?.Invoke(0);
        }
    }
}
