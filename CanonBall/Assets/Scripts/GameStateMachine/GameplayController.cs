using Assets.Scripts.Input;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.GameStateMachine
{
    public class GameplayController
    {
        private readonly CameraSystem _cameraSystem;

        private IController _controller;

        public GameplayController(CameraSystem cameraSystem, IController controller)
        {
            _cameraSystem = cameraSystem;
            PrivateSet(controller);
            _cameraSystem.ChangeCameraViewType(Camera.ViewType.FirstPerson);
        }

        public void SetController(IController controller)
        {
            _controller.Stop();
            PrivateSet(controller);
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
            _controller = controller;
            var preset = _controller.GetCameraTransformPreset();
            _cameraSystem.ApplyMainCameraPreset(preset);
        }
    }
}
