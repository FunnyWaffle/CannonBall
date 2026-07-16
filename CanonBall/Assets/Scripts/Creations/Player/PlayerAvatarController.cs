using Assets.Scripts.Camera;
using Assets.Scripts.Creations.Player.Components;
using Assets.Scripts.GameStateMachine.PlayerControl;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using System;
using UnityEngine;

namespace Assets.Scripts.Creations.Player
{
    public class PlayerAvatarController : IPlayerAvatarController, ISpawnable, IPoolableObject, IComponent
    {
        private readonly PlayerAvatarView _view;
        private readonly PlayerAvatarMover _mover;
        private readonly SpatialObject _spatialObject;

        public event EventHandler<ItemTypes> Disabled;

        public PlayerAvatarController(PlayerAvatarView view,
            PlayerAvatarMover mover,
            SpatialObject spatialObject)
        {
            _view = view;

            _mover = mover;
            _spatialObject = spatialObject;
        }

        public void Move(Vector2 movementInput)
        {
            var isGrounded = _view.IsGrounded;
            _mover.UpdateHorizontalVelocity(movementInput, isGrounded);
            _mover.UpdateVerticalSpeed(isGrounded);
            var velocity = _mover.GetVelocity();
            _view.Move(velocity);

            _spatialObject.ChangePosition(_view.Position);
        }

        public void Rotate(Vector3 positionToRotation)
        {
            var direction = Vector3.Normalize(positionToRotation - _view.ModelPosition);

            var flatDirection = direction;
            flatDirection.y = 0;

            var rotation = Quaternion.LookRotation(flatDirection, Vector3.up);
            _view.SetModelRotation(rotation);
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
            var jumped = _mover.ApplyJumpToVelocity(_view.IsGrounded);
            if (jumped)
                _view.EnableJumpAnimation();
        }

        public void Enable()
        {
            _view.Enable();
        }

        public void Place(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            _view.SetPosition(position);
            _view.SetRotation(rotation);
            _view.SetParent(parent);
        }
    }
}
