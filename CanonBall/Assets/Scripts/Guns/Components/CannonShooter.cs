using System;
using UnityEngine;

namespace Assets.Scripts.Guns.Components
{
    public class CannonShooter
    {
        private float _shootPower = 15f;
        private float _shootDelay = 1.5f;

        private float _nextPermittedShootingTime;

        public CannonShooter(float shootPower, float shootDelay)
        {
            _shootPower = shootPower;
            _shootDelay = shootDelay;
        }

        public Vector3 BarrelExitPosition { get; set; }
        public Vector3 BarrelForward { get; set; }
        public float ShootPower => _shootPower;

        public event Action<float> Shot;

        public void SetShootPower(float value)
        {
            _shootPower = value;
        }

        public void SetShootDelay(float value)
        {
            _shootDelay = value;
        }

        public void Shoot()
        {
            if (Time.time < _nextPermittedShootingTime)
                return;

            Shot?.Invoke(_shootPower);

            _nextPermittedShootingTime = Time.time + _shootDelay;
        }
    }
}