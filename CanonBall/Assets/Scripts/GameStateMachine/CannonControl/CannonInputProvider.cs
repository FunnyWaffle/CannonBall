using Assets.Scripts.Camera;
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
            ActiveCannonControllerContainer controllerContainer,
            CannonInput input,
            Aimer aimer,
            CameraSystem cameraSystem,
            CrosshairSystem crosshairSystem)
        {
            _controllerContainer = controllerContainer;
            _input = input;
            _aimer = aimer;
            _cameraSystem = cameraSystem;
            _crosshairSystem = crosshairSystem;

            _input.ShootPerform += OnShoot;
            _input.ViewModeActionPerformed += OnViewModePerformed;
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

        private void OnViewModePerformed(int index)
        {
            var viewType = index switch
            {
                0 => ViewType.FirstPerson,
                1 => ViewType.ThirdPerson,
                _ => throw new System.NotImplementedException("There's no view mode asign by index: " + index),
            };

            _cameraSystem.ChangeCameraViewType(viewType);
            _crosshairSystem.SwitchCrosshairMode(viewType);
        }
    }
}
