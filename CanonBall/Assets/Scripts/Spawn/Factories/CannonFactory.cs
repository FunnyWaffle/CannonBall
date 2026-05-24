using Assets.Scripts.Guns;
using Assets.Scripts.Guns.Components;
using Assets.Scripts.Interaction;
using Assets.Scripts.Shop;
using Assets.Scripts.Space;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Spawn.Factories
{
    public class CannonFactory : IFactory<CannonController>
    {
        private readonly DiContainer _container;
        private readonly CannonColliderMap _cannonColliderMap;
        private readonly SpatialGrid _spatialGrid;

        public ItemTypes CreationType => ItemTypes.Cannon;

        public CannonFactory(
            DiContainer container,
            CannonColliderMap cannonColliderMap,
            SpatialGrid spatialGrid)
        {
            _container = container;
            _cannonColliderMap = cannonColliderMap;
            _spatialGrid = spatialGrid;
        }

        public CannonController Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var obj = GameObject.Instantiate(prefab, position, rotation, parent);
            var view = obj.GetComponent<CannonView>();
            view.Initialize();

            var rotator = CreateRotator(view);
            var shooter = CreateShooter(view);
            var controller = _container.Instantiate<CannonController>(new object[] { view, rotator, shooter });

            _cannonColliderMap.Register(view.Colliders, controller);
            _spatialGrid.Add(controller);

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
