using Assets.Scripts.Camera;
using Assets.Scripts.Creations;
using Assets.Scripts.Explosion;
using Assets.Scripts.Guns.Projectile;
using Assets.Scripts.Input;
using Assets.Scripts.Interaction;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using Assets.Scripts.Systems;
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

        public CannonController(CannonCore core,
            CannonView cannonView,
            InteractionObjectsRepositiory interactionObjectsRepositiory,
            ExplosionHandler explosionHandler)
        {
            _core = core;
            _view = cannonView;

            _core.Shooter.Shot += OnShot;

            _interactionObjectsRepositiory = interactionObjectsRepositiory;
            _explosionHandler = explosionHandler;

            SetCannonToInteractionObjects();
        }

        public CrosshairTypes CrosshairType => CrosshairTypes.Cannon;

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

        public void HandleInput(Vector2 movementInput, Vector3 positionToRotation)
        {
            _core.RotateToPosition(positionToRotation);

            if (input.IsAttacked)
                _core.Shoot();
        }

        public CameraPresetHandler GetCameraTransformPreset()
        {
            return _view.CameraPresetHandler;
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

        private async void OnShot(float shootPower)
        {
            var projectile = _view.Projectile;
            SpawnRequested.Invoke(this, new SpawnArguments(ItemTypes.Ball,
               _view.BarrelExitPosition + projectile.Radius * _view.BarrelExitForward, _view.BarrelExitRotation));
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
