using Assets.Scripts.Config;
using Assets.Scripts.Creations;
using Assets.Scripts.Input;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.GameStateMachine
{
    public class PlayerAvatarMovementInputProvider : IUpdatable
    {
        private readonly PlayerAvatarInput _input;
        private readonly Aimer _aimer;
        private readonly CameraSystem _cameraSystem;
        private readonly CurrentPlayerAvatarController _controllerPlaceholder;

        public PlayerAvatarMovementInputProvider(
            PlayerAvatarInput input,
            Aimer aimer,
            CameraSystem cameraSystem,
            CurrentPlayerAvatarController controllerPlaceholder)
        {
            _input = input;
            _aimer = aimer;
            _cameraSystem = cameraSystem;
            _controllerPlaceholder = controllerPlaceholder;
        }

        public void Update()
        {
            var lookInput = _input.Look;
            var rotation = _aimer.Aim(lookInput);

            _cameraSystem.RotateCameraPivot(rotation);
            var position = _cameraSystem.MainCamera.GetFacedPosition(QueryTriggerInteraction.Ignore, LayerIds.BitMaskPlayer);

            var controller = _controllerPlaceholder.GetController();

            controller.Rotate(position);
            var movementInput = _input.Movement;
            controller.Move(movementInput);
        }
    }
}
