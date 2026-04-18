using Assets.Scripts.Explosion;
using System;
using UnityEngine;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieHitbox : IExplosionReceiver
    {
        private readonly Collider _collider;

        private Transform _transform;

        public ZombieHitbox(Collider collider)
        {
            _collider = collider;
            _transform = _collider.transform;
        }

        public Vector3 Position => _transform.position;
        public Collider Collider => _collider;

        public event Action<float, Vector3, float> ExplosionReceived;

        public void OnExplosion(float force, Vector3 position, float radius)
        {
            ExplosionReceived?.Invoke(force, position, radius);
        }

        public void SetPosition(Vector3 position)
        {
            _transform.position = position;
        }

        public void SetTrigger(bool isTrigger)
        {
            _collider.isTrigger = isTrigger;
        }
    }
}
