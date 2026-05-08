using Assets.Scripts.Systems;
using System.Threading.Tasks;

namespace Assets.Scripts.Config
{
    public class ConfigRepository
    {
        public PlayerConfig PlayerConfig { get; private set; }
        public AssetReferences PrefabAssetReferences { get; private set; }
        public AssetReferences SpriteAssetReferences { get; private set; }

        public async Task LoadAsync()
        {
            PlayerConfig = await ConfigLoader.Load<PlayerConfig>(ConfigNames.Player);
            PrefabAssetReferences = await ConfigLoader.Load<AssetReferences>(ConfigNames.PrefabAssetReferences);
            SpriteAssetReferences = await ConfigLoader.Load<AssetReferences>(ConfigNames.SpriteAssetReferences);
        }
    }
}
