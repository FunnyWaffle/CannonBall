using Assets.Scripts.Camera;
using Assets.Scripts.Combat;
using Assets.Scripts.GameStateMachine.CannonControl;
using Assets.Scripts.Guns.Components;
using Assets.Scripts.Shop;
using Assets.Scripts.Space;
using Assets.Scripts.Spawn;
using System;
using UnityEngine;

namespace Assets.Scripts.Guns
{
    public class CannonController : ICannonController, ISpawnable, IPoolableObject,
        ISpatialObject
    {
        private readonly CannonView _view;
        private readonly CannonRotator _rotator;
        private readonly CannonShooter _shooter;
        private readonly Health _health;

        public CannonController(
            CannonView cannonView,
            CannonRotator cannonRotator,
            CannonShooter shooter,
            Health health)
        {
            _view = cannonView;
            _rotator = cannonRotator;
            _shooter = shooter;
            _health = health;

            _health.Died += OnDead;
        }

        public Vector3 Position => _view.Position;

        public event Action<Vector3, Quaternion> Died;
        public event EventHandler<ItemTypes> Disabled;
        public event EventHandler<Vector3> PositionChanged;

        public void Enable()
        {
            _view.Enable();
            _health.Died += OnDead;
        }

        public void Place(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            _view.SetPosition(position);
            _view.SetRotation(rotation);
            _view.SetParent(parent);
        }

        public void Rotate(Vector3 positionToRotation)
        {
            var barrelExitPosition = _view.BarrelExitPosition;
            float shootPower = _view.ShootPower;
            var rotation = _rotator.Rotate(positionToRotation, barrelExitPosition, _view.BarrelExitForward, shootPower);
            _view.SetBarrelRotation(rotation);
        }

        public void Shoot()
        {
            _ = _shooter.Shoot(_view.BarrelExitPosition, _view.BarrelExitRotation, _view.Colliders);
        }

        public CameraPresetHandler GetCameraTransformPreset()
        {
            return _view.CameraPresetHandler;
        }

        private void OnDead()
        {
            Disable();
            Died?.Invoke(_view.Position, _view.BarrelLocalRotation);
        }

        private void Disable()
        {
            _health.Died -= OnDead;
            _view.Disable();
            Disabled?.Invoke(this, ItemTypes.Cannon);
        }
    }
}
