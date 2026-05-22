using Assets.Scripts.Config;
using Assets.Scripts.Creations;
using Assets.Scripts.Input;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.GameStateMachine.CannonControl
{
    public class CannonInputProvider : IUpdatable
    {
        private readonly ActiveCannonControllerContainer _controllerContainer;
        private readonly CannonInput _input;
        private readonly Aimer _aimer;
        private readonly CameraSystem _cameraSystem;
        private readonly CrosshairSystem _crosshairSystem;

        public CannonInputProvider(
            ActiveCannonControllerContainer _controllerContainer,
            CannonInput input,
            Aimer aimer,
            CameraSystem cameraSystem,
            CrosshairSystem crosshairSystem)
        {
            this._controllerContainer = _controllerContainer;
            _input = input;
            _aimer = aimer;
            _cameraSystem = cameraSystem;
            _crosshairSystem = crosshairSystem;

            _input.ShootPerform += OnShoot;
        }

        public void Update()
        {
            if (!_controllerContainer.TryGetController(out var controller))
                return;

            var lookInput = _input.Look;
            var rotation = _aimer.Aim(lookInput);

            _cameraSystem.RotateCameraPivot(rotation);
            var position = _cameraSystem.MainCamera.GetFacedPosition(QueryTriggerInteraction.Ignore, LayerIds.BitMaskGun);

            controller.Rotate(position);
        }

        private void OnShoot()
        {
            if (_controllerContainer.TryGetController(out var controller))
                controller.Shoot();
        }
    }
}
