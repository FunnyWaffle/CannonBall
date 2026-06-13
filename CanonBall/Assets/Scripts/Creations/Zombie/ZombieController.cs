using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using System;
using UnityEngine;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieController : ISpawnable, IPoolableObject
    {
        private readonly ZombieView _view;
        private readonly ZombieMover _mover;
        private readonly ZombieRagdoll _ragdoll;
        private readonly ZombieModel _model;
        private readonly ZombieHitbox _hitbox;
        private readonly ZombieAttacker _attacker;
        private readonly ZombieTargetSearch _zombieTargetSearch;

        private Vector3 _ragdollRootOffsetPosition;
        private Vector3 _hitboxOffsetPosition;

        public ZombieController(ZombieView view,
            ZombieMover zombieMover,
            ZombieRagdoll zombieRagdoll,
            ZombieModel zombieModel,
            ZombieHitbox zombieHitbox,
            ZombieAttacker zombieAttacker,
            ZombieTargetSearch zombieTargetSearch)
        {
            _view = view;
            _mover = zombieMover;
            _ragdoll = zombieRagdoll;
            _model = zombieModel;
            _hitbox = zombieHitbox;
            _attacker = zombieAttacker;
            _zombieTargetSearch = zombieTargetSearch;

            _ragdollRootOffsetPosition = _ragdoll.Position - _mover.Position;
            _hitboxOffsetPosition = _hitbox.Position - _mover.Position;

            Enable();
        }

        public ZombieHitbox Hitbox => _hitbox;

        public event EventHandler Died;
        public event EventHandler<ItemTypes> Disabled;

        public void Enable()
        {
            _view.Enable();
            ResetState();

            _ragdoll.FellAsleep += OnRagdollFellAsleep;
            _hitbox.ExplosionReceived += OnExplosion;
        }

        public void Place(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            _view.Place(position, rotation, parent);
        }

        public void LateUpdate()
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

        public void Update()
        {
            if (_attacker.CanAttack(_view.ModelCenterPosition))
                _view.EnableAttackAnimation();

            _mover.UpdatePath();

            _mover.UpdateMovementAnimation();
        }

        public void ResearchTarget()
        {
            _zombieTargetSearch.TrySearchTarget(_view.ModelCenterPosition, _view.AgentRadius);
        }

        private void OnRagdollFellAsleep()
        {
            Disable();
        }

        private void OnExplosion(float force, Vector3 position, float radius)
        {
            _mover.DisableAgent();
            _attacker.Disable();
            _ragdoll.ApplyExplosion(force, position, radius);
            _hitbox.SetTrigger(true);

            Died?.Invoke(this, EventArgs.Empty);
        }

        private void Disable()
        {
            _ragdoll.FellAsleep -= OnRagdollFellAsleep;
            _hitbox.ExplosionReceived -= OnExplosion;

            Disabled?.Invoke(this, ItemTypes.Zombie);
        }

        private void ResetState()
        {
            var ragdollPosition = _ragdoll.Position;
            _mover.SetPosition(ragdollPosition);
            _ragdoll.SetPosition(ragdollPosition + _ragdollRootOffsetPosition);
            _hitbox.SetPosition(ragdollPosition + _hitboxOffsetPosition);

            _mover.EnableAgent();
            _attacker.Enable();
            _hitbox.SetTrigger(true);
        }
    }
}
