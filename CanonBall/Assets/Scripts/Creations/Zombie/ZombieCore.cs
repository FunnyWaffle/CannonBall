using Assets.Scripts.Spawn;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieCore : MonoBehaviour, IPoolableObject
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        [SerializeField] private Vector3 _targetPosition;

        [SerializeField] private CapsuleCollider _collider;

        [SerializeField] private Rigidbody[] _rigidbodies;

        [SerializeField] private Transform _modelTransform;

        private ZombieMover _mover;
        private ZombieRagdoll _ragdoll;
        private ZombieHitbox _hitbox;
        private ZombieModel _model;

        private Vector3 _ragdollRootOffsetPosition;
        private Vector3 _hitboxOffsetPosition;

        public ZombieHitbox Hitbox => _hitbox;

        public event EventHandler Died;
        public event EventHandler Disabled;

        private void Awake()
        {
            _mover = new ZombieMover(_agent, _animator, _targetPosition);
            _ragdoll = new ZombieRagdoll(_rigidbodies);
            _model = new ZombieModel(_modelTransform);
            _hitbox = new(_collider);

            _ragdollRootOffsetPosition = _ragdoll.Position - _mover.Position;
            _hitboxOffsetPosition = _hitbox.Position - _mover.Position;
        }

        public void Enable()
            => gameObject.SetActive(true);

        private void Update()
        {
            _mover.Update();
        }

        private void LateUpdate()
        {
            if (_mover.IsAgentEnable)
            {
                Vector3 position = _mover.Position;
                _model.SetPosition(position);
                _hitbox.SetPosition(position);
            }
            else
            {
                _hitbox.SetPosition(_ragdoll.Position);
            }
        }

        private void OnEnable()
        {
            ResetState();

            _ragdoll.FellAsleep += OnRagdollFellAsleep;
            _hitbox.ExplosionReceived += OnExplosion;
        }

        private void OnDisable()
        {
            _ragdoll.FellAsleep += OnRagdollFellAsleep;
            _hitbox.ExplosionReceived += OnExplosion;

            Disabled?.Invoke(this, new EventArgs());
        }

        private void ResetState()
        {
            var ragdollPosition = _ragdoll.Position;
            _mover.SetPosition(ragdollPosition);
            _ragdoll.SetPosition(ragdollPosition + _ragdollRootOffsetPosition);
            _hitbox.SetPosition(ragdollPosition + _hitboxOffsetPosition);

            _mover.EnableAgent();
            _hitbox.SetTrigger(true);
        }

        private void Disable()
            => gameObject.SetActive(false);


        private void OnExplosion(float force, Vector3 position, float radius)
        {
            _mover.DisableAgent();
            _ragdoll.ApplyExplosion(force, position, radius);
            _hitbox.SetTrigger(true);

            Died?.Invoke(this, new EventArgs());
        }

        private void OnRagdollFellAsleep()
        {
            Disable();
        }

        private void OnValidate()
        {

        }
    }
}
