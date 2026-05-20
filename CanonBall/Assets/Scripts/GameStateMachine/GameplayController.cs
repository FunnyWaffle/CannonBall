using Assets.Scripts.Input;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.GameStateMachine
{
    public class GameplayController
    {
        private readonly CameraSystem _cameraSystem;
        private readonly CrosshairSystem _crosshairSystem;

        private IController _controller;
        private IController _previousController;

        public GameplayController(CameraSystem cameraSystem, CrosshairSystem crosshairSystem, IController controller)
        {
            _cameraSystem = cameraSystem;
            _crosshairSystem = crosshairSystem;
            PrivateSet(controller);
            _cameraSystem.ChangeCameraViewType(Camera.ViewType.FirstPerson);
        }

        public void SetController(IController controller)
        {
            _controller.Stop();
            _previousController = _controller;
            PrivateSet(controller);
        }

        public void ReturnPreviousController()
        {
            SetController(_previousController);
            _previousController = null;
        }

        public void HandleMovement(Vector2 movementInput)
        {
            _controller.Move(movementInput);
        }

        public void HandleRotation(Vector3 positionToRotation)
        {
            _controller.Rotate(positionToRotation);
        }

        public void HandleAttack()
        {
            _controller.Attack();
        }

        private void PrivateSet(IController controller)
        {
            var preset = controller.GetCameraTransformPreset();
            _cameraSystem.ApplyMainCameraPreset(preset);
            _crosshairSystem.EnableCrosshair(controller.CrosshairType);
            _controller = controller;
        }
    }
}
