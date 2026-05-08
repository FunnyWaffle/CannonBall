using Assets.Scripts.Config;
using Assets.Scripts.Shop;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Spawn
{
    public class AssetLoader
    {
        private readonly Dictionary<ItemTypes, Transform> _loadedPrefabs = new();
        private readonly Dictionary<ItemTypes, Sprite> _loadedSprites = new();

        private readonly AssetReferences _prefabAssetReferences;
        private readonly AssetReferences _spriteAssetReferences;

        public AssetLoader(ConfigRepository configRepository)
        {
            _prefabAssetReferences = configRepository.PrefabAssetReferences;
            _spriteAssetReferences = configRepository.SpriteAssetReferences;
        }

        public async Task<Sprite> LoadSprite(ItemTypes itemName)
        {
            if (_loadedSprites.TryGetValue(itemName, out var prefab))
                return prefab;

            var task = Addressables.LoadAssetAsync<Sprite>(_spriteAssetReferences.Get(itemName));
            prefab = await task.Task;

            _loadedSprites.Add(itemName, prefab);

            return prefab;
        }

        public async Task<Transform> Load(ItemTypes itemName)
        {
            if (_loadedPrefabs.TryGetValue(itemName, out var prefab))
                return prefab;

            var task = Addressables.LoadAssetAsync<GameObject>(_prefabAssetReferences.Get(itemName));
            var gameObject = await task.Task;
            prefab = gameObject.transform;

            _loadedPrefabs.Add(itemName, prefab);

            return prefab;
        }
    }
}
