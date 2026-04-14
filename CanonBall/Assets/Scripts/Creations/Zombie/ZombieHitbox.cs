using Assets.Scripts.Explosion;
using System;
using UnityEngine;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieHitbox : MonoBehaviour, IExplosionReceiver
    {
        [SerializeField] private CapsuleCollider _collider;

        private Transform _transform;

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

        private void Awake()
        {
            _transform = _collider.transform;
        }
    }
}
