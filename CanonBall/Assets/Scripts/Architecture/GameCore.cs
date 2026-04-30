using Assets.Scripts.Explosion;
using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Input;
using Assets.Scripts.Interaction;
using Assets.Scripts.PlayerData;
using Assets.Scripts.Spawn;
using Assets.Scripts.Systems;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class GameCore
    {
        public GameCore(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<Spawner>().FromComponentInHierarchy().AsSingle();
            container.BindInterfacesAndSelfTo<ParticleSpawnExecutor>().FromComponentInHierarchy().AsSingle();
            container.BindInterfacesAndSelfTo<EnemySpawnZone>().FromComponentInHierarchy().AsSingle();
            container.BindInterfacesAndSelfTo<WavesExecutor>().FromComponentInHierarchy().AsSingle();
            container.BindInterfacesAndSelfTo<ExplosionHandler>().AsSingle();
            container.BindInterfacesAndSelfTo<Store>().AsSingle();
            container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle();
            container.BindInterfacesAndSelfTo<InteractionObjectsRepositiory>().AsSingle();
            container.BindInterfacesAndSelfTo<InventoryController>().AsSingle();
            container.BindInterfacesAndSelfTo<UIState>().AsSingle();
            container.BindInterfacesAndSelfTo<GameplayState>().AsSingle();
            container.BindInterfacesAndSelfTo<GameStateMachine.GameStateMachine>().AsSingle();
            container.BindInterfacesAndSelfTo<PlayerInputHandler>().AsSingle();
            container.BindInterfacesAndSelfTo<Updater>().FromComponentInHierarchy().AsSingle();

            container.Resolve<ExplosionHandler>().Exploded +=
            container.Resolve<ParticleSpawnExecutor>().ExecuteExplosionParticlesSpawn;
        }
    }
}
