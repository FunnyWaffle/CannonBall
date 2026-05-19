using Assets.Scripts.Explosion;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Guns.Projectile
{
    public class Ball : MonoBehaviour, IProjectile, ISpawnable, IPoolableObject, IExplosionMaker
    {
        [SerializeField] private float _explosionPower = 5f;
        [SerializeField] private float _explosionRadius = 2f;
        [SerializeField] private float _lifeTime;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private SphereCollider _sphereCollider;

        private readonly List<Collider> _ignoredColliders = new();

        private float _colliderRaduis;
        private float _currentLifeTime;

        public bool _isExploded = false;

        public float Radius => _colliderRaduis;
        public SphereCollider Collider => _sphereCollider;

        public event EventHandler<ItemTypes> Disabled;
        public event Action<float, Vector3, float> ExplosionPerformed;

        public void SetForce(float forceValue)
        {
            _rigidbody.linearVelocity = transform.forward * forceValue;
            //_rigidbody.AddForce(new Vector3(0f, 0f, _startPower), ForceMode.Impulse);
        }

        public void SetIgnoredCollider(Collider collider)
        {
            Physics.IgnoreCollision(collider, _sphereCollider);
            _ignoredColliders.Add(collider);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Place(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            transform.SetLocalPositionAndRotation(position, rotation);
            transform.SetParent(parent);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _colliderRaduis = Collider.radius;
        }

        private void Update()
        {
            _currentLifeTime += Time.deltaTime;

            if (_currentLifeTime >= _lifeTime)
                Disable();
        }

        private void OnEnable()
        {
            ResetState();
        }

        private void OnDisable()
        {
            Disabled?.Invoke(this, ItemTypes.Ball);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Explode(collision.contacts[0].point);
            Disable();
        }

        private void Explode(Vector3 explosionCenter)
        {
            if (_isExploded)
                return;

            _isExploded = true;

            ExplosionPerformed?.Invoke(_explosionPower, explosionCenter, _explosionRadius);
        }

        private void Disable()
        {
            gameObject.SetActive(false);
            Collider.enabled = false;
        }

        private void ResetState()
        {
            Collider.enabled = true;
            _isExploded = false;
            _currentLifeTime = 0;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.linearVelocity = Vector3.zero;
            _ignoredColliders.Clear();
            ClearIgnoredColliders();
        }

        private void ClearIgnoredColliders()
        {
            foreach (var collider in _ignoredColliders)
            {
                Physics.IgnoreCollision(collider, _sphereCollider, false);
            }
            _ignoredColliders.Clear();
        }
    }
}
