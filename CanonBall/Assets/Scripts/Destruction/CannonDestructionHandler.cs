using Assets.Scripts.Guns;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using UnityEngine;

namespace Assets.Scripts.Destruction
{
    public class CannonDestructionHandler
    {
        private readonly AssetLoader _assetLoader;

        public CannonDestructionHandler(AssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public void Register(CannonController cannonController)
        {
            cannonController.Died += SpawnBrokenCannon;
        }

        public void Unregister(CannonController cannonController)
        {
            cannonController.Died -= SpawnBrokenCannon;
        }

        private async void SpawnBrokenCannon(Vector3 position, Quaternion rotation)
        {
            var prefab = await _assetLoader.Load(ItemTypes.BrokenCannon);
            GameObject.Instantiate(prefab, position, rotation);
        }
    }
}
