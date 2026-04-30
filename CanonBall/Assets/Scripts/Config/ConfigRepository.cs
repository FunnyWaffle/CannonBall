using Assets.Scripts.Systems;
using System.Threading.Tasks;

namespace Assets.Scripts.Config
{
    public class ConfigRepository
    {
        public PlayerConfig PlayerConfig { get; private set; }

        public async Task LoadAsync()
        {
            PlayerConfig = await ConfigLoader.Load<PlayerConfig>(ConfigNames.Player);
        }
    }
}
