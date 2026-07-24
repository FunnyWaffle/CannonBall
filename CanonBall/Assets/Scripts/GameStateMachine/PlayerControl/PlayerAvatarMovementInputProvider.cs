using Assets.Scripts.Camera;
using Assets.Scripts.Config;
using Assets.Scripts.Creations;
using Assets.Scripts.Input;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.GameStateMachine.PlayerControl
{
    public class PlayerAvatarMovementInputProvider : IUpdatable
    {
        private readonly PlayerAvatarInput _input;
        private readonly Aimer _aimer;
        private readonly CameraSystem _cameraSystem;
        private readonly ActivePlayerAvatarControllerContainer _currentController;

        public PlayerAvatarMovementInputProvider(
            PlayerAvatarInput input,
            Aimer aimer,
            CameraSystem cameraSystem,
            ActivePlayerAvatarControllerContainer currentController)
        {
            _input = input;
            _aimer = aimer;
            _cameraSystem = cameraSystem;
            _currentController = currentController;

            _input.JumpActionPerformed += OnJumpPerform;
            _input.ViewModeActionPerformed += OnViewModePerformed;
        }

        public void Update()
        {
            if (!_currentController.TryGetController(out var controller))
                return;

            var lookInput = _input.Look;
            var rotation = _aimer.Aim(lookInput);

            _cameraSystem.RotateCameraPivot(rotation);
            var position = _cameraSystem.TryGetMainCameraFacedPosition(QueryTriggerInteraction.Ignore, LayerIds.BitMaskPlayer);

            controller.Rotate(position);
            var movementInput = _input.Movement;
            controller.Move(movementInput);
        }

        private void OnJumpPerform()
        {
            if (!_currentController.TryGetController(out var controller))
                return;

            controller.Jump();
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
        }
    }
}
