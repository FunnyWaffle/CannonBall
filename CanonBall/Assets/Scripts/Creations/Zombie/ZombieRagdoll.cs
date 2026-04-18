using Assets.Scripts.Systems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieRagdoll
    {
        private readonly Rigidbody[] _rigidbodies;
        private readonly List<Collider> _colliders = new();
        private readonly Transform _rootBone;

        private Coroutine _explosionCoroutine;

        public ZombieRagdoll(Rigidbody[] rigidbodies)
        {
            _rigidbodies = rigidbodies;

            _rootBone = _rigidbodies[0].transform;
            GetRigidbodyColliders();
            DisableRagdoll();
        }

        public Vector3 Position => _rootBone.position;
        public Vector3 Down => -_rootBone.up;

        public event Action FellAsleep;

        public void ApplyExplosion(float force, Vector3 position, float radius)
        {
            EnableRagdoll((rigidbody) => { rigidbody.AddExplosionForce(force, position, radius); });
            _explosionCoroutine = CoroutineRunner.Instance.StartCoroutine(WaitForExplosionEnd());
        }

        public void SetPosition(Vector3 position)
        {
            _rootBone.position = position;
        }

        private void GetRigidbodyColliders()
        {
            foreach (var rigidbody in _rigidbodies)
            {
                if (rigidbody.gameObject.TryGetComponent<Collider>(out var collider))
                {
                    _colliders.Add(collider);
                }
            }
        }

        private void EnableRagdoll(Action<Rigidbody> explosion)
        {
            foreach (var collider in _colliders)
            {
                collider.enabled = true;
            }

            foreach (var rigidbody in _rigidbodies)
            {
                rigidbody.isKinematic = false;
                explosion.Invoke(rigidbody);
            }
        }

        private void DisableRagdoll()
        {
            foreach (var rigidbody in _rigidbodies)
            {
                rigidbody.isKinematic = true;
            }

            foreach (var collider in _colliders)
            {
                collider.enabled = false;
            }
        }

        private IEnumerator WaitForExplosionEnd()
        {
            while (true)
            {
                var isAllRigidbodySleeping = true;
                foreach (var rigidbody in _rigidbodies)
                {
                    if (!rigidbody.IsSleeping())
                    {
                        isAllRigidbodySleeping = false;
                        break;
                    }
                }

                if (isAllRigidbodySleeping)
                    break;

                yield return null;
            }

            FellAsleep?.Invoke();
            DisableRagdoll();
        }
    }
}
