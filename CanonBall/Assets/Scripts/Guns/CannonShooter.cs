using Assets.Scripts.Guns.Projectile;
using Assets.Scripts.Spawn;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Guns
{
    public class CannonShooter : MonoBehaviour
    {
        [SerializeField] private float _shootPower = 15f;
        [SerializeField] private Ball _projectile;
        [SerializeField] private Transform _barrelExit;

        [Inject] private Spawner _spawner;

        public Vector3 BarrelExitPosition => _barrelExit.position;
        public float ShootPower => _shootPower;

        public void Shoot()
        {
            var ball = _spawner.Spawn(_projectile,
                _barrelExit.position + _projectile.Radius * _barrelExit.forward,
                _barrelExit.rotation);

            ball.SetForce(_shootPower);
        }

        public Vector3 GetFacedPosition()
        {
            if (Physics.Raycast(_barrelExit.position, _barrelExit.forward, out var hit, float.PositiveInfinity))
                return hit.point;
            else
                return _barrelExit.position + _barrelExit.forward * 10f;
        }
    }
}