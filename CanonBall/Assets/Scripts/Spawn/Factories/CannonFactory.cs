using Assets.Scripts.Config;
using Assets.Scripts.Creations.Player.Components;
using Assets.Scripts.Guns;
using Assets.Scripts.Guns.Components;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Spawn.Factories
{
    public class CannonFactory : IFactory
    {
        private readonly DiContainer _container;
        private readonly ConfigRepository _configRepository;

        public CannonFactory(DiContainer container, ConfigRepository configRepository)
        {
            _configRepository = configRepository;
            _container = container;
        }

        public Type CreatedType => typeof(CannonController);

        public IPoolableObject Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var obj = GameObject.Instantiate(prefab, position, rotation, parent);
            var view = obj.GetComponent<CannonView>();
            view.Initialize();

            var rotator = CreateRotator(view);
            var shooter = CreateShooter(view);
            var aimer = CreateAimer(rotation);
            var core = new CannonCore(rotator, shooter, aimer);
            return _container.Instantiate<CannonController>(new object[] { core, view });
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

        private Aimer CreateAimer(Quaternion rotation)
        {
            var aimer = new Aimer(_configRepository.PlayerConfig.Sensitivity, rotation.eulerAngles);

            return aimer;
        }
    }
}
