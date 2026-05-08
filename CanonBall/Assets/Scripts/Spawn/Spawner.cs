using Assets.Scripts.Shop;
using Assets.Scripts.Spawn.Factories;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Spawn
{
    public class Spawner<T>
        where T : class, ISpawnable, IPoolableObject
    {
        private readonly IFactory<T> _factory;
        private readonly ObjectPool _objectPool;
        private readonly AssetLoader _prefabLoader;

        public Spawner(AssetLoader prefabLoader, ObjectPool objectPool, IFactory<T> factory)
        {
            _prefabLoader = prefabLoader;
            _objectPool = objectPool;
            _factory = factory;
        }

        public async Task<T> Spawn(ItemTypes itemType, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (_objectPool.TryGet<T>(out var obj))
            {
                obj.Place(position, rotation, parent);
            }
            else
            {
                var prefab = await _prefabLoader.Load(itemType);

                obj = _factory.Create(prefab, position, rotation, parent);
                _objectPool.Register(obj);
            }

            return obj;
        }
    }
}
