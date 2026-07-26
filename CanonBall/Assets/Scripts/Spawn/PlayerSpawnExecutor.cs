using Assets.Scripts.Creations.Player;
using Assets.Scripts.GameStateMachine.PlayerControl;
using Assets.Scripts.Shop;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Spawn
{
    public class PlayerSpawnExecutor
    {
        private readonly Spawner<PlayerAvatarController> _spawner;
        private readonly ActivePlayerAvatarControllerContainer _activePlayerAvatarControllerContainer;

        public PlayerSpawnExecutor(Spawner<PlayerAvatarController> spawner,
            ActivePlayerAvatarControllerContainer activePlayerAvatarControllerContainer)
        {
            _spawner = spawner;
            _activePlayerAvatarControllerContainer = activePlayerAvatarControllerContainer;

            _ = Spawn();
        }

        private async Task Spawn()
        {
            var playerAvatarController = await _spawner.Spawn(ItemType.PlayerAvatar, new Vector3(10, 300, 10), Quaternion.identity);
            _activePlayerAvatarControllerContainer.SetController(playerAvatarController);
        }
    }
}
