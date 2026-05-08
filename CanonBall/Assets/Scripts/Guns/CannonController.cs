using Assets.Scripts.Camera;
using Assets.Scripts.Creations;
using Assets.Scripts.Explosion;
using Assets.Scripts.Guns.Projectile;
using Assets.Scripts.Input;
using Assets.Scripts.Interaction;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using Assets.Scripts.Systems;
using R3;
using System;
using UnityEngine;

namespace Assets.Scripts.Guns
{
    public class CannonController : IUpdatable, IController, ISpawnable, IPoolableObject, ISpawnRequester<Ball>
    {
        private readonly CannonCore _core;
        private readonly CannonView _view;

        private readonly InteractionObjectsRepositiory _interactionObjectsRepositiory;
        private readonly ExplosionHandler _explosionHandler;
        private readonly CompositeDisposable _disposables = new();

        public CannonController(CannonCore core,
            CannonView cannonView,
            InteractionObjectsRepositiory interactionObjectsRepositiory,
            ExplosionHandler explosionHandler)
        {
            _core = core;
            _view = cannonView;

            _disposables.Add(_core.CurrentViewType.Subscribe(OnCameraViewTypeChange));
            _disposables.Add(_core.Aimer.Rotation.Subscribe(OnRotationChanged));

            _core.Shooter.Shot += OnShot;
            _core.CrosshairModeChanged += OnCrosshairModeChange;
            _core.CrosshairPositionChanged += OnCrosshairPositionChange;

            _interactionObjectsRepositiory = interactionObjectsRepositiory;
            _explosionHandler = explosionHandler;

            SetCannonToInteractionObjects();
        }

        public event EventHandler Disabled;
        public event EventHandler<SpawnArguments> SpawnRequested;

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

        public void Update()
        {
            _core.Shooter.BarrelExitPosition = _view.BarrelExitPosition;
            _core.Shooter.BarrelForward = _view.BarrelExitForward;
        }

        public void HandleInput(InputData input)
        {
            _core.Aim(input.Rotation);
            var position = CameraSystem.MainCamera.GetFacedPosition();
            _core.RotateToPosition(position);

            if (input.IsAttacked)
                _core.Shoot();

            _core.SetCrosshairType(input.ViewModeIndex);
            _core.SetViewMode(input.ViewModeIndex);
        }

        public void TransferCamera()
        {
            CameraSystem.ApplyMainCameraPreset(_view.CameraPreset);
            _view.ShowCrosshair(_core.CurrentCrosshairMode);
        }

        public void SetSpawnedObject(Ball ball)
        {
            foreach (var collider in _view.Colliders)
            {
                Physics.IgnoreCollision(collider, ball.Collider);
            }

            ball.SetForce(_core.Shooter.ShootPower);
            _explosionHandler.AddExplosionMaker(ball);
        }

        private void OnCrosshairModeChange(CrosshairMode mode)
        {
            switch (mode)
            {
                case CrosshairMode.FirstPerson:
                    _view.FirstPersonCrosshairPreview.SetActive(true);
                    _view.ThirdPersonCrosshairPreview.SetActive(false);
                    break;
                case CrosshairMode.ThirdPerson:
                    _view.FirstPersonCrosshairPreview.SetActive(false);
                    _view.ThirdPersonCrosshairPreview.SetActive(true);
                    break;
            }
        }

        private void OnCrosshairPositionChange(Vector3 position)
        {
            if (_core.CurrentCrosshairMode == CrosshairMode.FirstPerson)
                _view.FirstPersonCrosshairPreview.SetPosition(position);
            else
                _view.ThirdPersonCrosshairPreview.SetPosition(position);
        }

        private async void OnShot(float shootPower)
        {
            var projectile = _view.Projectile;
            SpawnRequested.Invoke(this, new SpawnArguments(ItemTypes.Ball, _view.BarrelExitPosition + projectile.Radius * _view.BarrelExitForward,
               _view.BarrelExitRotation, null));
        }

        private void OnCameraViewTypeChange(CameraViewType type)
        {
            _view.SetCameraViewType(type);
            CameraSystem.ApplyMainCameraPreset(_view.CameraPreset);
        }

        private void OnRotationChanged(Quaternion quaternion)
        {
            _view.SetCameraPivotRotation(quaternion);
        }

        private void SetCannonToInteractionObjects()
        {
            foreach (var collider in _view.Colliders)
            {
                _interactionObjectsRepositiory.AddControllers(collider, this);
            }
        }
    }
}
