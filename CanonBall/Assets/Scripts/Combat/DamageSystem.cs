using Assets.Scripts.Creations;
using Assets.Scripts.Space;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class DamageSystem
    {
        private readonly World _world;

        public DamageSystem(World world)
        {
            _world = world;
        }

        public void DealDamage(Collider target, float value)
        {
            if (!_world.SpatialObjectsMap.TryGetValue(target, out var spatialObject)
                || !_world.EntityComponents.TryGetValue(spatialObject, out var components)
                || !components.TryGet<IDamageable>(out var damageable))
                return;

            damageable.TakeDamage(value);
        }
    }
}
