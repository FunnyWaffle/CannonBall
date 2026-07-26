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
        private readonly Dictionary<ItemType, Transform> _loadedPrefabs = new();
        private readonly Dictionary<ItemType, Sprite> _loadedSprites = new();

        private readonly AssetReferences _prefabReferences;
        private readonly AssetReferences _spriteReferences;

        public AssetLoader(ConfigRepository configRepository)
        {
            _prefabReferences = configRepository.PrefabReferences;
            _spriteReferences = configRepository.SpriteReferences;
        }

        public async Task<Sprite> LoadSprite(ItemType itemType)
        {
            if (_loadedSprites.TryGetValue(itemType, out var sprite))
                return sprite;

            var task = Addressables.LoadAssetAsync<Sprite>(_spriteReferences.Get(itemType));
            sprite = await task.Task;

            _loadedSprites[itemType] = sprite;

            return sprite;
        }

        public async Task<Transform> Load(ItemType itemType)
        {
            if (_loadedPrefabs.TryGetValue(itemType, out var prefab))
                return prefab;

            var task = Addressables.LoadAssetAsync<GameObject>(_prefabReferences.Get(itemType));
            var gameObject = await task.Task;
            prefab = gameObject.transform;

            _loadedPrefabs[itemType] = prefab;

            return prefab;
        }
    }
}
