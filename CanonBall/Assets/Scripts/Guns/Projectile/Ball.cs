using Assets.Scripts.Explosion;
using Assets.Scripts.Spawn;
using System;
using UnityEngine;

namespace Assets.Scripts.Guns.Projectile
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class Ball : MonoBehaviour, IPoolableObject, IExplosionMaker
    {
        [SerializeField] private float _explosionPower = 5f;
        [SerializeField] private float _explosionRadius = 2f;
        [SerializeField] private float _lifeTime;
        [SerializeField] private Rigidbody _rigidbody;

        private SphereCollider _sphereCollider;

        private float _colliderRaduis;
        private float _currentLifeTime;

        public bool _isExploded = false;

        public float Radius => _colliderRaduis;
        public SphereCollider Collider => _sphereCollider;

        public event EventHandler Disabled;
        public event Action<float, Vector3, float> ExplosionPerformed;

        public void SetForce(float forceValue)
        {
            _rigidbody.linearVelocity = transform.forward * forceValue;
            //_rigidbody.AddForce(new Vector3(0f, 0f, _startPower), ForceMode.Impulse);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _sphereCollider = GetComponent<SphereCollider>();
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
            Disabled?.Invoke(this, new EventArgs());
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
        }
    }
}
