using Assets.Scripts.Spawn;
using System;
using UnityEngine;

namespace Assets.Scripts.Player.Cannon.Projectile
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour, IPoolableObject
    {
        [SerializeField] private float _startPower;
        [SerializeField] private float _lifeTime;
        [SerializeField] private Rigidbody _rigidbody;

        private float _currentLifeTime;

        public event EventHandler ObjectLifeEnded;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            Activate();
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
        }

        private void OnCollisionEnter(Collision collision)
        {
            Disable();
        }

        public void Activate()
        {
            ResetState();
            gameObject.SetActive(true);
            _rigidbody.linearVelocity = transform.forward * _startPower;
            //_rigidbody.AddForce(new Vector3(0f, 0f, _startPower), ForceMode.Impulse);
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }

        private void ResetState()
        {
            _currentLifeTime = 0;
        }
    }
}
