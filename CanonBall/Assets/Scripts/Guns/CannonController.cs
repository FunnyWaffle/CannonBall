using Assets.Scripts.Camera;
using Assets.Scripts.Config;
using Assets.Scripts.Creations;
using Assets.Scripts.Creations.Player.Components;
using Assets.Scripts.Explosion;
using Assets.Scripts.Guns.Components;
using Assets.Scripts.Input;
using Assets.Scripts.Interaction;
using Assets.Scripts.Spawn;
using Assets.Scripts.Systems;
using R3;
using UnityEngine;

namespace Assets.Scripts.Guns
{
    public class CannonController : IUpdatable, IGameplayController
    {
        private readonly CannonCore _core;
        private readonly CannonView _view;

        private readonly InteractionObjectsRepositiory _interactionObjectsRepositiory;
        private readonly Spawner _spawner;
        private readonly ExplosionHandler _explosionHandler;
        private readonly CompositeDisposable _disposables = new();

        public CannonController(CannonView cannonView,
            InteractionObjectsRepositiory interactionObjectsRepositiory,
            Spawner spawner,
            ExplosionHandler explosionHandler,
            ConfigRepository configRepository)
        {
            _view = cannonView;
            _view.Initialize();

            var rotator = CreateRotator();
            var shooter = CreateShooter();
            var aimer = CreateAimer(configRepository);
            _core = new CannonCore(rotator, shooter, aimer);

            _disposables.Add(_core.CurrentViewType.Subscribe(OnCameraViewTypeChange));
            _core.CrosshairModeChanged += OnCrosshairModeChange;
            _core.CrosshairPositionChanged += OnCrosshairPositionChange;

            _interactionObjectsRepositiory = interactionObjectsRepositiory;
            _spawner = spawner;
            _explosionHandler = explosionHandler;

            SetCannonToInteractionObjects();
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
            _view.ShowCrosshair(_core.CrosshairMode);
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
            if (_core.CrosshairMode == CrosshairMode.FirstPerson)
                _view.FirstPersonCrosshairPreview.SetPosition(position);
            else
                _view.ThirdPersonCrosshairPreview.SetPosition(position);
        }

        private void OnShot(float shootPower)
        {
            var projectile = _view.Projectile;
            var ball = _spawner.Spawn(projectile,
                _view.BarrelExitPosition + projectile.Radius * _view.BarrelExitForward,
                _view.BarrelExitRotation);

            foreach (var collider in _view.Colliders)
            {
                Physics.IgnoreCollision(collider, ball.Collider);
            }

            ball.SetForce(shootPower);
            _explosionHandler.AddExplosionMaker(ball);
        }

        private void OnCameraViewTypeChange(CameraViewType type)
        {
            _view.SetCameraViewType(type);
            CameraSystem.ApplyMainCameraPreset(_view.CameraPreset);
        }

        private void SetCannonToInteractionObjects()
        {
            foreach (var collider in _view.Colliders)
            {
                _interactionObjectsRepositiory.AddCannon(collider, this);
            }
        }

        private CannonRotator CreateRotator()
        {
            var rotator = new CannonRotator(
                _view.RotationSpeed,
                _view.PitchAngleLimit,
                _view.BarrelLocalRotation);

            rotator.Rotated += _view.SetBarrelRotation;
            _view.RotationSpeedChanged += rotator.SetRotationSpeed;
            _view.PitchLimitChanged += rotator.SetPitchLimit;

            return rotator;
        }

        private CannonShooter CreateShooter()
        {
            var shooter = new CannonShooter(_view.ShootPower, _view.ShootDelay);

            shooter.Shot += OnShot;
            _view.ShootPowerChanged += shooter.SetShootPower;
            _view.ShootDelayChanged += shooter.SetShootDelay;

            return shooter;
        }

        private Aimer CreateAimer(ConfigRepository configRepository)
        {
            var aimer = new Aimer(configRepository.PlayerConfig.Sensitivity, _view.CameraPreset.Pivot.eulerAngles);

            aimer.RotationChanged += _view.SetCameraPivotRotation;

            return aimer;
        }
    }
}
