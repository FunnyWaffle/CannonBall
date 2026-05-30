using Assets.Scripts.Camera;
using Assets.Scripts.Config;
using Assets.Scripts.Creations.Player.Components;
using Assets.Scripts.GameStateMachine;
using UnityEngine;

namespace Assets.Scripts.Creations.Player
{
    public class PlayerAvatarController : IPlayerAvatarController
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
            _mover.Speed = config.Speed;
            _mover.JumpPower = config.JumpPower;
            _mover.MaxVelocity = config.MaxVelocity;
        }

        public void Move(Vector2 movementInput)
        {
            _mover.UpdateHorizontalVelocity(movementInput);
            _mover.UpdateVerticalSpeed(_view.IsGrounded);
            var velocity = _mover.GetVelocity();
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

        public void Stop()
        {
            _mover.Stop();
            _view.Stop();
        }

        public void Jump()
        {
            _mover.ApplyJumpToVelocity(_view.IsGrounded);
        }
    }
}
