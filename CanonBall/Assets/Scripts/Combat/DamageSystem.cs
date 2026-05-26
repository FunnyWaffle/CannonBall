using Assets.Scripts.Creations;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class DamageSystem
    {
        private readonly Dictionary<Collider, IDamageable> _damageables = new();

        public void DealDamage(Collider target, float value)
        {
            if (!_damageables.TryGetValue(target, out var damageable))
                return;

            damageable.TakeDamage(value);
        }

        public void Register(Collider collider, IDamageable damageable)
        {
            _damageables[collider] = damageable;
        }
        public void Register(IEnumerable<Collider> colliders, IDamageable damageable)
        {
            foreach (var collider in colliders)
            {
                _damageables[collider] = damageable;
            }
        }

        public void Unregister(Collider collider)
        {
            _damageables.Remove(collider);
        }
    }
}
