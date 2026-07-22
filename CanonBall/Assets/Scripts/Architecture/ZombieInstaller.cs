using Assets.Scripts.Creations.Zombie;
using Assets.Scripts.EnemyAttractionObjects;
using Assets.Scripts.Spawn;
using Assets.Scripts.Spawn.Factories;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class ZombieInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<WavesExecutor>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySpawnZone>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<ZombieUpdater>().AsSingle();

            Container.BindInterfacesAndSelfTo<EnemyAttractionObject>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<ZombieFactory>().AsSingle();
        }
    }
}