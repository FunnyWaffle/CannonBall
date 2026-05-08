using Assets.Scripts.Camera;
using Assets.Scripts.Creations.Player;
using Assets.Scripts.Explosion;
using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Guns;
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
        Container.BindInterfacesAndSelfTo<Shop>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo<ObjectPool>().AsSingle();
        Container.BindInterfacesAndSelfTo<AssetLoader>().AsSingle();
        Container.BindInterfacesAndSelfTo<ZombieFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<CannonFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<Spawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<ParticleSpawnExecutor>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<EnemySpawnZone>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<WavesExecutor>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<ExplosionHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<InteractionObjectsRepositiory>().AsSingle();
        Container.BindInterfacesAndSelfTo<InventoryController>().AsSingle();
        Container.BindInterfacesAndSelfTo<UIController>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameplayController>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerInputHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlaceObjectSystem>().AsSingle();
        Container.BindInterfacesAndSelfTo<Updater>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo<PlayerAvatarController>().AsSingle();

        Container.Resolve<ExplosionHandler>().Exploded +=
        Container.Resolve<ParticleSpawnExecutor>().ExecuteExplosionParticlesSpawn;

        Container.Resolve<InventoryController>();

        var mainCamera = Container.Resolve<MainCamera>();
        CameraSystem.SetMainCamera(mainCamera);
    }
}