using Assets.Scripts.Placement;
using Assets.Scripts.Spawn;
using Assets.Scripts.Spawn.Factories;
using Assets.Scripts.Spawn.Pools;
using Assets.Scripts.Spawn.Projectile;
using Assets.Scripts.Systems;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class SpawnInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo(typeof(ObjectPool<>)).AsTransient();
            Container.BindInterfacesAndSelfTo<AssetLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo(typeof(Spawner<>)).AsTransient();
            Container.BindInterfacesAndSelfTo<ProjectileSpawner>().AsTransient();

            Container.BindInterfacesAndSelfTo<BallFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<NotForVehicleZoneFactiory>().AsSingle();

            Container.BindInterfacesAndSelfTo<ParticleSpawnExecutor>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<UniversalSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<UniversalPool>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlaceObjectSystem>().AsSingle();
        }
    }
}