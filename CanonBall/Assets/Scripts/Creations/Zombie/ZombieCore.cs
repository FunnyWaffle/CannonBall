using Assets.Scripts.Spawn;
using System;
using UnityEngine;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieCore : MonoBehaviour, IPoolableObject
    {
        [SerializeField] private ZombieMover _mover;
        [SerializeField] private ZombieRagdoll _ragdoll;
        [SerializeField] private ZombieHitbox _hitbox;
        [SerializeField] private ZombieModel _model;

        public ZombieHitbox Hitbox => _hitbox;

        public event EventHandler Died;
        public event EventHandler Disabled;

        public void Awake()
        {
            _ragdoll.FellAsleep += OnRagdollFellAsleep;
            Hitbox.ExplosionReceived += OnExplosion;
        }

        public void Enable()
            => gameObject.SetActive(true);

        private void LateUpdate()
        {
            if (_mover.IsAgentEnable)
            {
                Vector3 position = _mover.Position;
                _model.SetPosition(position);
                Hitbox.SetPosition(position);
            }
            else
            {
                Hitbox.SetPosition(_ragdoll.Position);
            }
        }

        private void OnEnable()
        {
            ResetState();
        }

        private void OnDisable()
        {
            Disabled?.Invoke(this, new EventArgs());
        }

        private void ResetState()
        {
            _mover.EnableAgent();
            Hitbox.SetTrigger(false);
        }

        private void Disable()
            => gameObject.SetActive(false);


        private void OnExplosion(float force, Vector3 position, float radius)
        {
            _mover.DisableAgent();
            _ragdoll.ApplyExplosion(force, position, radius);
            Hitbox.SetTrigger(true);

            Died?.Invoke(this, new EventArgs());
        }

        private void OnRagdollFellAsleep()
        {
            Disable();
        }
    }
}
