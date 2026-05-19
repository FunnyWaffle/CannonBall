using Assets.Scripts.Shop;
using Assets.Scripts.Spawn.Factories;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Spawn
{
    public class Spawner<T>
        where T : ISpawnable, IPoolableObject
    {
        private readonly Dictionary<ItemTypes, IFactory<T>> _factories = new();
        private readonly ObjectPool<T> _objectPool;
        private readonly AssetLoader _prefabLoader;

        public Spawner(AssetLoader prefabLoader, ObjectPool<T> objectPool, params IFactory<T>[] factories)
        {
            _prefabLoader = prefabLoader;
            _objectPool = objectPool;

            foreach (var factory in factories)
            {
                _factories[factory.CreationType] = factory;
            }
        }

        public async Task<T> Spawn(ItemTypes itemType, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (_objectPool.TryGet(itemType, out var obj))
            {
                obj.Place(position, rotation, parent);
            }
            else
            {
                var prefab = await _prefabLoader.Load(itemType);

                var factory = _factories[itemType];
                obj = factory.Create(prefab, position, rotation, parent);
                _objectPool.Register(obj);
            }

            return obj;
        }
    }
}
