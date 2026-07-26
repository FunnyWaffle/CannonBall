using Assets.Scripts.Guns;
using Assets.Scripts.Shop;
using Assets.Scripts.Space;
using Assets.Scripts.Spawn;
using UnityEngine;

namespace Assets.Scripts.Destruction
{
    public class CannonDestructionHandler
    {
        private readonly AssetLoader _assetLoader;
        private readonly SpatialGrid _spatialGrid;

        public CannonDestructionHandler(AssetLoader assetLoader, SpatialGrid spatialGrid)
        {
            _assetLoader = assetLoader;
            _spatialGrid = spatialGrid;
        }

        public void Register(CannonController cannonController)
        {
            cannonController.Died += SpawnBrokenCannon;
        }

        public void Unregister(CannonController cannonController)
        {
            cannonController.Died -= SpawnBrokenCannon;
        }

        private async void SpawnBrokenCannon(object sender, CannonDeathEventArgs cannonDeathEventArgs)
        {
            _spatialGrid.Remove(sender as ISpatialObject);

            var prefab = await _assetLoader.Load(ItemType.BrokenCannon);
            GameObject.Instantiate(prefab, cannonDeathEventArgs.Position, cannonDeathEventArgs.Rotation);

            // Добавлять сломанную пушку в SpatialGrid
        }
    }
}
