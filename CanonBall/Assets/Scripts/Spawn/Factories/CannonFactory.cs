using Assets.Scripts.Combat;
using Assets.Scripts.Creations;
using Assets.Scripts.Destruction;
using Assets.Scripts.Guns;
using Assets.Scripts.Guns.Components;
using Assets.Scripts.Shop;
using Assets.Scripts.Space;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Spawn.Factories
{
    public class CannonFactory : IFactory<CannonController>
    {
        private readonly DiContainer _container;
        private readonly World _world;
        private readonly SpatialGrid _spatialGrid;
        private readonly CannonDestructionHandler _cannonDestructionHandler;

        public ItemTypes CreationType => ItemTypes.Cannon;

        public CannonFactory(
            DiContainer container,
            World world,
            SpatialGrid spatialGrid,
            CannonDestructionHandler cannonDestructionHandler)
        {
            _container = container;
            _world = world;
            _spatialGrid = spatialGrid;
            _cannonDestructionHandler = cannonDestructionHandler;
        }

        public CannonController Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var obj = GameObject.Instantiate(prefab, position, rotation, parent);
            var view = obj.GetComponent<CannonView>();
            view.Initialize();

            var rotator = CreateRotator(view);
            var shooter = CreateShooter(view);
            var hitBox = new HitBox(view, view.AttackZoneCorners, view.Colliders);
            var health = new Health(50, 50);

            var controller = _container.Instantiate<CannonController>(
                new object[] { view, rotator, shooter, health });

            var components = new EntityComponents();
            components.Add(rotator);
            components.Add(shooter);
            components.Add(hitBox);
            components.Add(health);
            components.Add(controller);
            components.Add(view);
            _world.EntityComponents[controller] = components;

            foreach (var collider in view.Colliders)
            {
                _world.SpatialObjectsMap[collider] = controller;
            }

            _spatialGrid.Add(controller);
            _cannonDestructionHandler.Register(controller);

            return controller;
        }

        private CannonRotator CreateRotator(CannonView view)
        {
            var rotator = _container.Instantiate<CannonRotator>(
                new object[] {
                view.RotationSpeed,
                view.PitchAngleLimit,
                view.BarrelLocalRotation
            });

            view.RotationSpeedChanged += rotator.SetRotationSpeed;
            view.PitchLimitChanged += rotator.SetPitchLimit;

            return rotator;
        }

        private CannonShooter CreateShooter(CannonView view)
        {
            var shooter = _container.Instantiate<CannonShooter>(new object[] { view.ShootPower, view.ShootDelay });

            view.ShootPowerChanged += shooter.SetShootPower;
            view.ShootDelayChanged += shooter.SetShootDelay;

            return shooter;
        }
    }
}
