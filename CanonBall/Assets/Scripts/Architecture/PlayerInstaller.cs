using Assets.Scripts.Crosshairs;
using Assets.Scripts.GameStateMachine.PlayerControl;
using Assets.Scripts.Input;
using Assets.Scripts.Spawn;
using Assets.Scripts.Spawn.Factories;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputSystem_Actions.PlayerActions>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerAvatarInput>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerCrosshair>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerAvatarMovementInputProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerAvatarAttackInputProvider>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ActivePlayerAvatarControllerContainer>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerAvatarInteractionInputProvider>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerSpawnExecutor>().AsSingle().NonLazy();
        }
    }
}