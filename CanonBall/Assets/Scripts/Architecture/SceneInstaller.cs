using Assets.Scripts.Camera;
using Assets.Scripts.Creations.Player;
using Assets.Scripts.Creations.Player.Components;
using Assets.Scripts.Crosshairs;
using Assets.Scripts.Curency;
using Assets.Scripts.Explosion;
using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Guns;
using Assets.Scripts.Guns.Projections;
using Assets.Scripts.Input;
using Assets.Scripts.Interaction;
using Assets.Scripts.PlayerData;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using Assets.Scripts.Spawn.Factories;
using Assets.Scripts.Spawn.Projectile;
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
        Container.BindInterfacesAndSelfTo<PlayerInteractionService>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo<ParticleSpawnExecutor>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<EnemySpawnZone>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<WavesExecutor>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<Updater>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo(typeof(ObjectPool<>)).AsTransient();
        Container.BindInterfacesAndSelfTo<AssetLoader>().AsSingle();
        Container.BindInterfacesAndSelfTo(typeof(Spawner<>)).AsTransient();
        Container.BindInterfacesAndSelfTo(typeof(SpawnRequesterCreationHandler<>)).AsTransient();
        Container.BindInterfacesAndSelfTo<ProjectileSpawner>().AsTransient();

        Container.BindInterfacesAndSelfTo<CannonFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<ZombieFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<BallFactory>().AsSingle();

        Container.BindInterfacesAndSelfTo<CannonProjectionFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<Spawner<CannonProjection>>().AsSingle();
        Container.BindInterfacesAndSelfTo<SpawnRequestHandler<CannonProjection>>().AsSingle();
        Container.BindInterfacesAndSelfTo<SpawnRequestHandler<CannonController>>().AsSingle();


        Container.BindInterfacesAndSelfTo<WaveCurrencyAccruer>().AsSingle();
        Container.BindInterfacesAndSelfTo<ExplosionHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<InventoryController>().AsSingle();
        Container.BindInterfacesAndSelfTo<UIController>().AsSingle();
        Container.BindInterfacesAndSelfTo<Aimer>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
        Container.BindInterfacesAndSelfTo<CameraSystem>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlaceObjectSystem>().AsSingle();
        Container.BindInterfacesAndSelfTo<CrosshairSystem>().AsSingle();
        Container.BindInterfacesAndSelfTo<PurchaseHandler>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<PlayerAvatarMover>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAvatarController>().AsSingle();

        Container.BindInterfacesAndSelfTo<InteractionObjectsRepositiory>().AsSingle();
        Container.BindInterfacesAndSelfTo<CannonColliderMap>().AsSingle();

        Container.BindInterfacesAndSelfTo<InputSystem_Actions>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputSystem_Actions.PlayerActions>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputSystem_Actions.CannonActions>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAvatarInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<CannonInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputSystem>().AsSingle();
        Container.BindInterfacesAndSelfTo<CannonInputProvider>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAvatarInputProvider>().AsSingle();

        Container.Resolve<ExplosionHandler>().Exploded +=
        Container.Resolve<ParticleSpawnExecutor>().ExecuteExplosionParticlesSpawn;

        Container.Resolve<InventoryController>();

        Container.Resolve<SpawnRequestHandler<CannonProjection>>();
        Container.Resolve<SpawnRequestHandler<CannonController>>();

        Container.Resolve<WaveCurrencyAccruer>();

        Container.Resolve<InputSystem>();
    }
}