using Assets.Scripts.Config;
using Assets.Scripts.Creations;
using Assets.Scripts.Input;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.GameStateMachine
{
    public class CannonInputProvider : IUpdatable
    {
        private readonly CannonInput _input;
        private readonly Aimer _aimer;
        private readonly CameraSystem _cameraSystem;
        private readonly CrosshairSystem _crosshairSystem;

        private ICannonController _controller;

        public CannonInputProvider(CannonInput input, Aimer aimer, CameraSystem cameraSystem, CrosshairSystem crosshairSystem)
        {
            _input = input;
            _aimer = aimer;
            _cameraSystem = cameraSystem;
            _crosshairSystem = crosshairSystem;

            _input.ShootPerform += OnShoot;
        }

        public void SetController(ICannonController controller)
        {
            var preset = controller.GetCameraTransformPreset();
            _cameraSystem.ApplyMainCameraPreset(preset);
            _crosshairSystem.EnableCrosshair(CrosshairTypes.Cannon);
            _controller = controller;
        }

        public void ExitController()
        {
            _controller = null;
        }

        public void HandleRotation(Vector3 positionToRotation)
        {
            _controller.Rotate(positionToRotation);
        }

        public void HandleAttack()
        {
            _controller.Shoot();
        }

        public void Update()
        {
            if (_controller == null)
                return;

            var lookInput = _input.Look;
            var rotation = _aimer.Aim(lookInput);

            _cameraSystem.RotateCameraPivot(rotation);
            var position = _cameraSystem.MainCamera.GetFacedPosition(QueryTriggerInteraction.Ignore, LayerIds.BitMaskGun);

            _controller.Rotate(position);
        }

        private void OnShoot()
        {
            _controller.Shoot();
        }
    }
}
