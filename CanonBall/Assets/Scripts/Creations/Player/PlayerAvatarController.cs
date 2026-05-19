using Assets.Scripts.Camera;
using Assets.Scripts.Config;
using Assets.Scripts.Creations.Player.Components;
using Assets.Scripts.Input;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.Creations.Player
{
    public class PlayerAvatarController : IController
    {
        private readonly PlayerAvatarMover _mover;
        private readonly PlayerAvatarView _view;

        public PlayerAvatarController(PlayerAvatarView view,
            PlayerAvatarMover mover,
            PlayerConfig config)
        {
            _view = view;
            _view.Initialize();

            _mover = mover;
            _mover.SetSpeed(config.Speed);
        }

        public CrosshairTypes CrosshairType => CrosshairTypes.Player;

        public void Move(Vector2 movementInput)
        {
            var velocity = _mover.UpdateVelocity(movementInput);
            _view.Move(velocity);
        }

        public void Rotate(Vector3 positionToRotation)
        {
            var direction = Vector3.Normalize(positionToRotation - _view.ModelPosition);

            var flatDirection = direction;
            flatDirection.y = 0;

            var rotation = Quaternion.LookRotation(flatDirection, Vector3.up);
            _view.SetCameraPivotRotation(rotation);
        }

        public void Attack()
        {
        }

        public CameraPresetHandler GetCameraTransformPreset()
        {
            return _view.CameraPresetHandler;
        }
    }
}
