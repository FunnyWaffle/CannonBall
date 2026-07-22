using Assets.Scripts.Explosion;
using Assets.Scripts.Systems;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private MonoBehaviour[] _dependencies;
        public override void InstallBindings()
        {
            //foreach (var dependency in _dependencies)
            //{
            //    Container.BindInterfacesAndSelfTo(dependency.GetType()).FromInstance(dependency).AsSingle();

            //}

            Container.BindInterfacesAndSelfTo<Updater>().FromComponentInHierarchy().AsSingle();
        }

        public override void Start()
        {
            Container.Resolve<ExplosionHandler>().Exploded +=
            Container.Resolve<ParticleSpawnExecutor>().ExecuteExplosionParticlesSpawn;

            //Container.Resolve<InventoryController>();

            //Container.Resolve<WaveCurrencyAccruer>();
            //Container.Resolve<PlayerAvatarAttackInputProvider>();

            //Container.Resolve<InputSystem>();

            //Container.Resolve<InventoryInputProvider>();
            //Container.Resolve<ShopInputProvider>();

            //Container.Resolve<CannonExit>();

            //Container.Resolve<PlayerSpawnExecutor>();
        }
    }
}