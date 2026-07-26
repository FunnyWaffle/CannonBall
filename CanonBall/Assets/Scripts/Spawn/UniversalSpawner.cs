using Assets.Scripts.Creations;
using Assets.Scripts.Creations.Placement;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn.Factories;
using Assets.Scripts.Spawn.Pools;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Spawn
{
    public class UniversalSpawner
    {
        private readonly Dictionary<ItemType, IUniversalFactory> _factories = new();
        private readonly UniversalPool _pool;
        private readonly AssetLoader _assetLoader;

        public UniversalSpawner(UniversalPool pool, AssetLoader assetLoader, params IUniversalFactory[] factories)
        {
            _pool = pool;
            _assetLoader = assetLoader;

            foreach (var factory in factories)
            {
                _factories[factory.CreationType] = factory;
            }
        }

        public async Task<EntityComponents> SpawnAsync(ItemType itemType, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (_pool.TryGet(itemType, out var components))
            {
                if (components.TryGet<IPlaceable>(out var placeable))
                {
                    placeable.Place(position, rotation, parent);
                    return components;
                }
            }

            var prefab = await _assetLoader.Load(itemType);

            var factory = _factories[itemType];
            components = factory.Create(prefab, position, rotation, parent);
            _pool.TryRegister(components);

            return components;
        }
    }
}
