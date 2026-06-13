using Assets.Scripts.Combat;
using Assets.Scripts.Config;
using Assets.Scripts.Creations;
using Assets.Scripts.Creations.Player;
using Assets.Scripts.Creations.Player.Components;
using Assets.Scripts.Shop;
using Assets.Scripts.Space;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.Spawn.Factories
{
    public class PlayerFactory : IFactory<PlayerAvatarController>
    {
        private readonly CameraSystem _cameraSystem;
        private readonly SpatialGrid _spatialGrid;
        private readonly World _world;
        private readonly PlayerConfig _config;

        public ItemTypes CreationType => ItemTypes.PlayerAvatar;

        public PlayerFactory(
            CameraSystem cameraSystem,
            SpatialGrid spatialGrid,
            World world,
            PlayerConfig config)
        {
            _cameraSystem = cameraSystem;
            _spatialGrid = spatialGrid;
            _world = world;
            _config = config;
        }

        public PlayerAvatarController Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var instance = GameObject.Instantiate(prefab, position, rotation, parent);
            var view = instance.GetComponent<PlayerAvatarView>();
            view.Initialize();

            var mover = CreateMover();
            var spatialObject = new SpatialObject(view.Position);
            var hitbox = new HitBox(view.AttackCorners, view.Collider);
            var health = new Health(100, 100);
            var controller = new PlayerAvatarController(view, mover, spatialObject);
            var components = new EntityComponents();

            components.Add(mover);
            components.Add(hitbox);
            components.Add(health);
            components.Add(controller);
            components.Add(view);

            _spatialGrid.Add(spatialObject);
            _world.EntityComponents[spatialObject] = components;

            return controller;
        }

        private PlayerAvatarMover CreateMover()
        {
            return new PlayerAvatarMover(cameraSystem: _cameraSystem)
            {
                Speed = _config.Speed,
                JumpPower = _config.JumpPower,
                MaxVelocity = _config.MaxVelocity,
                MovementAcceleration = _config.MovementAcceleration,
                MovementDeceleration = _config.MovementDeceleration,
                MovementAirAcceleration = _config.MovementAirAcceleration,
                MovementAirDeceleration = _config.MovementAirDeceleration
            };
        }
    }
}
