using Assets.Scripts.Shop;
using Assets.Scripts.Spawn.Factories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Spawn
{
    public class Spawner
    {
        private readonly Dictionary<Type, IFactory> _factories = new();
        private readonly ObjectPool _objectPool;
        private readonly PrefabLoader _prefabLoader;

        public Spawner(PrefabLoader prefabLoader, ObjectPool objectPool, params IFactory[] factories)
        {
            _prefabLoader = prefabLoader;
            _objectPool = objectPool;

            foreach (var factory in factories)
            {
                _factories[factory.CreatedType] = factory;
            }
        }

        public async Task<T> Spawn<T>(ItemTypes itemType, Vector3 position, Quaternion rotation, Transform parent = null)
            where T : class, ISpawnable, IPoolableObject
        {
            if (_objectPool.TryGet<T>(out var obj))
            {
                obj.Place(position, rotation, parent);
            }
            else
            {
                var prefab = await _prefabLoader.Load(itemType);

                var factory = _factories[typeof(T)];
                obj = (T)factory.Create(prefab, position, rotation, parent);
                _objectPool.Register(obj);
            }

            return obj;
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
            => GameObject.Instantiate(prefab, position, rotation, parent);
    }
}
