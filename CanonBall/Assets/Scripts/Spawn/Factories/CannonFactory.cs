using Assets.Scripts.Guns;
using Assets.Scripts.Guns.Components;
using Assets.Scripts.Guns.Projectile;
using Assets.Scripts.Shop;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Spawn.Factories
{
    public class CannonFactory : IFactory<CannonController>, ISpawnRequesterCreator<Ball>
    {
        private readonly DiContainer _container;

        public ItemTypes CreationType => ItemTypes.Cannon;

        public CannonFactory(DiContainer container)
        {
            _container = container;
        }

        public event Action<ISpawnRequester<Ball>> SpawnRequesterCreated;

        public CannonController Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var obj = GameObject.Instantiate(prefab, position, rotation, parent);
            var view = obj.GetComponent<CannonView>();
            view.Initialize();

            var rotator = CreateRotator(view);
            var shooter = CreateShooter(view);
            var core = new CannonCore(rotator, shooter);
            var controller = _container.Instantiate<CannonController>(new object[] { core, view });

            SpawnRequesterCreated?.Invoke(controller);

            return controller;
        }

        private CannonRotator CreateRotator(CannonView view)
        {
            var rotator = new CannonRotator(
                view.RotationSpeed,
                view.PitchAngleLimit,
                view.BarrelLocalRotation);

            rotator.Rotated += view.SetBarrelRotation;
            view.RotationSpeedChanged += rotator.SetRotationSpeed;
            view.PitchLimitChanged += rotator.SetPitchLimit;

            return rotator;
        }

        private CannonShooter CreateShooter(CannonView view)
        {
            var shooter = new CannonShooter(view.ShootPower, view.ShootDelay);

            view.ShootPowerChanged += shooter.SetShootPower;
            view.ShootDelayChanged += shooter.SetShootDelay;

            return shooter;
        }
    }
}
