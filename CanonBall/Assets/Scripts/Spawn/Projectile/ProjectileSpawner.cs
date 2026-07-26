using Assets.Scripts.Guns.Projectile;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn.Factories;
using Assets.Scripts.Spawn.Pools;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Spawn.Projectile
{
    public class ProjectileSpawner
    {
        private readonly Dictionary<ItemType, IFactory<IProjectile>> _factories = new();
        private readonly ObjectPool<IProjectile> _objectPool;
        private readonly AssetLoader _prefabLoader;

        public ProjectileSpawner(ObjectPool<IProjectile> objectPool, AssetLoader prefabLoader, params IFactory<IProjectile>[] factories)
        {
            _objectPool = objectPool;
            _prefabLoader = prefabLoader;

            foreach (var factory in factories)
            {
                _factories[factory.CreationType] = factory;
            }
        }

        public async Task<IProjectile> Spawn(ItemType itemType, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (_objectPool.TryGet(itemType, out var obj))
            {
                obj.Place(position, rotation, parent);
            }
            else
            {
                var prefab = await _prefabLoader.Load(itemType);

                var forward = rotation * Vector3.forward;
                var collider = prefab.GetComponentInChildren<Collider>();
                var size = GetColliderSize(collider);
                var halfSize = size / 2;
                var offset = forward * halfSize.z;

                var factory = _factories[itemType];
                obj = factory.Create(prefab, position + offset, rotation, parent);
                _objectPool.Register(obj);
            }

            return obj;
        }

        private Vector3 GetColliderSize(Collider collider)
        {
            if (collider is SphereCollider sphereCollider)
            {
                var localScale = collider.transform.localScale;
                float radius = sphereCollider.radius * Mathf.Max(
                    localScale.x,
                    localScale.y,
                    localScale.z
                );

                return new Vector3(radius, radius, radius);
            }

            return Vector3.zero;
        }
    }
}
