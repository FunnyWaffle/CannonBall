using Assets.Scripts.Camera;
using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Guns.Components;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using System;
using UnityEngine;

namespace Assets.Scripts.Guns
{
    public class CannonController : ICannonController, ISpawnable, IPoolableObject
    {
        private readonly CannonView _view;
        private readonly CannonRotator _rotator;
        private readonly CannonShooter _shooter;

        public CannonController(CannonView cannonView, CannonRotator cannonRotator, CannonShooter shooter)
        {
            _view = cannonView;
            _rotator = cannonRotator;
            _shooter = shooter;
        }

        public event EventHandler<ItemTypes> Disabled;

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
    }
}
