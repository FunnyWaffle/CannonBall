using Assets.Scripts.Spawn;
using System;
using UnityEngine;

namespace Assets.Scripts.Player.Cannon.Projectile
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class Ball : MonoBehaviour, IPoolableObject
    {
        [SerializeField] private float _startPower;
        [SerializeField] private float _lifeTime;
        [SerializeField] private Rigidbody _rigidbody;

        private float _raduis;
        private float _currentLifeTime;

        public float Radius => _raduis;

        public event EventHandler ObjectLifeEnded;

        public void MakeFly()
        {
            _rigidbody.linearVelocity = transform.forward * _startPower;
            //_rigidbody.AddForce(new Vector3(0f, 0f, _startPower), ForceMode.Impulse);
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _raduis = GetComponent<SphereCollider>().radius;
        }

        private void Update()
        {
            _currentLifeTime += Time.deltaTime;

            if (_currentLifeTime >= _lifeTime)
                Disable();
        }

        private void OnDisable()
        {
            ObjectLifeEnded?.Invoke(this, new EventArgs());
            ResetState();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Disable();
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }

        private void ResetState()
        {
            _currentLifeTime = 0;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.linearVelocity = Vector3.zero;
        }
    }
}
