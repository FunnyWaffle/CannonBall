using Assets.Scripts.Creations;
using System;

namespace Assets.Scripts.Combat
{
    public class Health : IDamageable
    {
        private readonly float _maxHealth;

        private float _health;

        public Health(float maxHealth, float health)
        {
            _maxHealth = maxHealth;
            _health = health;
        }

        public event Action Died;

        public void TakeDamage(float value)
        {
            _health -= value;

            if (_health <= 0)
            {
                _health = 0;
                Died?.Invoke();
            }
        }

        public void Heal(float value)
        {
            _health += value;

            if (_health > _maxHealth)
                _health = _maxHealth;
        }
    }
}
