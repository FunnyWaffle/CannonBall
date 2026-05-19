using Assets.Scripts.Spawn.Projectile;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Guns.Components
{
    public class CannonShooter
    {
        private readonly ProjectileSpawner _spawner;

        private float _shootPower = 15f;
        private float _shootDelay = 1.5f;

        private float _nextPermittedShootingTime;

        public CannonShooter(ProjectileSpawner spawner, float shootPower, float shootDelay)
        {
            _spawner = spawner;
            _shootPower = shootPower;
            _shootDelay = shootDelay;
        }

        public void SetShootPower(float value)
        {
            _shootPower = value;
        }

        public void SetShootDelay(float value)
        {
            _shootDelay = value;
        }

        public async Task Shoot(Vector3 shootPosition, Quaternion shootRotation, Collider[] ignoredColliders)
        {
            if (Time.time < _nextPermittedShootingTime)
                return;

            var projectile = await _spawner.Spawn(Shop.ItemTypes.Ball, shootPosition, shootRotation);

            foreach (var collider in ignoredColliders)
            {
                projectile.SetIgnoredCollider(collider);
            }

            projectile.SetForce(_shootPower);

            _nextPermittedShootingTime = Time.time + _shootDelay;
        }
    }
}