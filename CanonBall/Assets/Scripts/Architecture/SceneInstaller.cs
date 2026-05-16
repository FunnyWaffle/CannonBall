using Assets.Scripts.Camera;
using Assets.Scripts.Creations.Player;
using Assets.Scripts.Creations.Zombie;
using Assets.Scripts.Crosshairs;
using Assets.Scripts.Curency;
using Assets.Scripts.Explosion;
using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Guns;
using Assets.Scripts.Guns.Projectile;
using Assets.Scripts.Guns.Projections;
using Assets.Scripts.Input;
using Assets.Scripts.Interaction;
using Assets.Scripts.PlayerData;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using Assets.Scripts.Spawn.Factories;
using Assets.Scripts.Systems;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private MonoBehaviour[] _dependencies;
    public override void InstallBindings()
    {
        //foreach (var dependency in _dependencies)
        //{
        //    Container.BindInterfacesAndSelfTo(dependency.GetType()).FromInstance(dependency).AsSingle();

        //}
        Container.BindInterfacesAndSelfTo<PlayerAvatarView>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<CannonView>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<MainCamera>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<InventoryView>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<ShopView>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<Vendor>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerCrosshair>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<CannonCrosshair>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<FirstPersonCannonCrosshairPreview>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<ThirdPersonCannonCrosshairPreview>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo<ParticleSpawnExecutor>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<EnemySpawnZone>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<WavesExecutor>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<Updater>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo<ObjectPool>().AsSingle();
        Container.BindInterfacesAndSelfTo<AssetLoader>().AsSingle();

        Container.BindInterfacesAndSelfTo<CannonFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<ZombieFactory>().AsSingle();

        CreateSpawnSystem<CannonController>();
        CreateSpawnSystem<ZombieController>();
        CreateSpawnSystem<Ball>();

        Container.BindInterfacesAndSelfTo<CannonProjectionFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<Spawner<CannonProjection>>().AsSingle();
        Container.BindInterfacesAndSelfTo<SpawnRequestHandler<CannonProjection>>().AsSingle();
        Container.BindInterfacesAndSelfTo<SpawnRequestHandler<CannonController>>().AsSingle();


        Container.BindInterfacesAndSelfTo<WaveCurrencyAccruer>().AsSingle();
        Container.BindInterfacesAndSelfTo<ExplosionHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<InteractionObjectsRepositiory>().AsSingle();
        Container.BindInterfacesAndSelfTo<InventoryController>().AsSingle();
        Container.BindInterfacesAndSelfTo<UIController>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameplayController>().AsSingle();
        Container.BindInterfacesAndSelfTo<Aimer>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerInputHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlaceObjectSystem>().AsSingle();
        Container.BindInterfacesAndSelfTo<CrosshairSystem>().AsSingle();
        Container.BindInterfacesAndSelfTo<PurchaseHandler>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<PlayerAvatarController>().AsSingle();

        Container.Resolve<ExplosionHandler>().Exploded +=
        Container.Resolve<ParticleSpawnExecutor>().ExecuteExplosionParticlesSpawn;

        Container.Resolve<InventoryController>();

        Container.Resolve<SpawnRequestHandler<CannonProjection>>();
        Container.Resolve<SpawnRequestHandler<CannonController>>();

        var mainCamera = Container.Resolve<MainCamera>();
        CameraSystem.SetMainCamera(mainCamera);
    }

    private void CreateSpawnSystem<TSpawnable>()
        where TSpawnable : ISpawnable, IPoolableObject
    {
        Container.BindInterfacesAndSelfTo<Spawner<TSpawnable>>().AsSingle();
        Container.BindInterfacesAndSelfTo<SpawnRequesterCreationHandler<TSpawnable>>().AsSingle();
    }
}