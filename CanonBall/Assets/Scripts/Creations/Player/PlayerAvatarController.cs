using Assets.Scripts.Camera;
using Assets.Scripts.Config;
using Assets.Scripts.Creations.Player.Components;
using Assets.Scripts.Input;
using Assets.Scripts.Systems;
using R3;
using UnityEngine;

namespace Assets.Scripts.Creations.Player
{
    public class PlayerAvatarController : IController
    {
        private readonly PlayerAvatarMover _mover;
        private readonly PlayerAvatarView _view;

        private readonly CompositeDisposable _disposables = new();

        public PlayerAvatarController(PlayerAvatarView view,
            PlayerConfig config)
        {
            _view = view;
            _view.Initialize();

            _mover = CreateMoverComtroller(config);
        }

        public CrosshairTypes CrosshairType => CrosshairTypes.Player;

        public void HandleInput(Vector2 movementInput, Vector3 positionToRotation)
        {
            RotateView(positionToRotation);
            _mover.UpdateVelocity(movementInput);
        }

        public CameraPresetHandler GetCameraTransformPreset()
        {
            return _view.CameraPresetHandler;
        }

        private void RotateView(Vector3 positionToRotation)
        {
            var direction = Vector3.Normalize(positionToRotation - _view.ModelPosition);

            var flatDirection = direction;
            flatDirection.y = 0;

            var rotation = Quaternion.LookRotation(flatDirection, Vector3.up);
            _view.SetCameraPivotRotation(rotation);
        }

        private void OnVelocityChange(Vector3 velocity)
        {
            var projectedVelocity = _mover.ProjectVelocityOn(_view.ModelForwad, _view.ModelRight);
            _view.Move(projectedVelocity);
        }

        private PlayerAvatarMover CreateMoverComtroller(PlayerConfig config)
        {
            var mover = new PlayerAvatarMover(config.Speed);

            mover.VelocityChanged += OnVelocityChange;

            return mover;
        }
    }
}
