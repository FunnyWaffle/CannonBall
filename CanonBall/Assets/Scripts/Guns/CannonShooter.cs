using Assets.Scripts.Explosion;
using Assets.Scripts.Guns.Projectile;
using Assets.Scripts.Spawn;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Guns
{
    public class CannonShooter : MonoBehaviour
    {
        [SerializeField] private float _shootPower = 15f;
        [SerializeField] private float _shootDelay = 1.5f;
        [SerializeField] private Ball _projectile;
        [SerializeField] private Transform _barrelExit;

        [Inject] private Spawner _spawner;
        [Inject] private ExplosionHandler _explosionHandler;

        private float _nextPermittedShootingTime;

        public Vector3 BarrelForward => _barrelExit.forward;
        public Vector3 BarrelExitPosition => _barrelExit.position;
        public float ShootPower => _shootPower;

        public void Shoot()
        {
            if (Time.time < _nextPermittedShootingTime)
                return;

            var ball = _spawner.Spawn(_projectile,
                _barrelExit.position + _projectile.Radius * _barrelExit.forward,
                _barrelExit.rotation);

            ball.SetForce(_shootPower);
            _explosionHandler.AddExplosionMaker(ball);

            _nextPermittedShootingTime = Time.time + _shootDelay;
        }
    }
}