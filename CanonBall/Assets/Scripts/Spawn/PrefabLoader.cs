using Assets.Scripts.Shop;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Spawn
{
    public class PrefabLoader
    {
        private readonly Dictionary<ItemTypes, Transform> _loadedPrefabs = new();
        private readonly Dictionary<ItemTypes, IPoolableObject> _loadedPoolableObjects = new();

        public async Task<T> Load<T>(ItemTypes itemName)
            where T : IPoolableObject
        {
            if (_loadedPoolableObjects.TryGetValue(itemName, out var prefab))
                return (T)prefab;

            var task = Addressables.LoadAssetAsync<T>(itemName);
            prefab = await task.Task;

            _loadedPoolableObjects.Add(itemName, prefab);

            return (T)prefab;
        }

        public async Task<Transform> Load(ItemTypes itemName)
        {
            if (_loadedPrefabs.TryGetValue(itemName, out var prefab))
                return prefab;

            var task = Addressables.LoadAssetAsync<GameObject>("Zombie");
            var gameObject = await task.Task;
            prefab = gameObject.transform;

            _loadedPrefabs.Add(itemName, prefab);

            return prefab;
        }
    }
}
