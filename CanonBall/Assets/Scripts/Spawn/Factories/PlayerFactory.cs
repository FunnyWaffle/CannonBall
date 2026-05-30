using Assets.Scripts.Config;
using Assets.Scripts.Creations.Player;
using Assets.Scripts.Creations.Player.Components;
using Assets.Scripts.Shop;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.Spawn.Factories
{
    public class PlayerFactory : IFactory<PlayerAvatarController>
    {
        private readonly CameraSystem _cameraSystem;
        private readonly PlayerConfig _config;

        public ItemTypes CreationType => ItemTypes.PlayerAvatar;

        public PlayerFactory(CameraSystem cameraSystem, PlayerConfig config)
        {
            _cameraSystem = cameraSystem;
            _config = config;
        }

        public PlayerAvatarController Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var instance = GameObject.Instantiate(prefab, position, rotation, parent);
            var view = instance.GetComponent<PlayerAvatarView>();

            var mover = new PlayerAvatarMover(cameraSystem: _cameraSystem);
            return new PlayerAvatarController(view, mover, _config);
        }
    }
}
