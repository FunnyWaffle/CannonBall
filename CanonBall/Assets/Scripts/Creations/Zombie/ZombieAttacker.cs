using Assets.Scripts.Combat;
using UnityEngine;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieAttacker
    {
        private readonly ZombieTarget _zombieTarget;
        private readonly DamageSystem _damageSystem;
        private readonly CollisionNotifier _rigthHandcollisionNotifier;
        private readonly CollisionNotifier _leftHandcollisionNotifier;

        private readonly float _damage = 10f;
        private readonly float _attackRange = .5f;
        private readonly float _attackRangeSqrt;

        public ZombieAttacker(
            ZombieTarget zombieTarget,
            DamageSystem damageSystem,
            CollisionNotifier rigthHandcollisionNotifier,
            CollisionNotifier leftHandcollisionNotifier)
        {
            _zombieTarget = zombieTarget;
            _damageSystem = damageSystem;
            _rigthHandcollisionNotifier = rigthHandcollisionNotifier;
            _leftHandcollisionNotifier = leftHandcollisionNotifier;

            _attackRangeSqrt = _attackRange * _attackRange;
        }

        public bool CanAttack(Vector3 currentPosition)
        {
            var target = _zombieTarget.Target;
            if (target == null)
                return false;

            var targetPosition = _zombieTarget.TargetPosition;
            targetPosition.y = currentPosition.y;

            var distanceSqrt = Vector3.SqrMagnitude(targetPosition - currentPosition);
            if (distanceSqrt > _attackRangeSqrt)
                return false;

            return true;
        }

        public void Attack(Collider collider)
        {
            _damageSystem.DealDamage(collider, _damage);
        }

        public void Enable()
        {
            _rigthHandcollisionNotifier.TriggerEntered += Attack;
            _leftHandcollisionNotifier.TriggerEntered += Attack;
        }

        public void Disable()
        {
            _rigthHandcollisionNotifier.TriggerEntered -= Attack;
            _leftHandcollisionNotifier.TriggerEntered -= Attack;
        }
    }
}
