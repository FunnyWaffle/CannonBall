using Assets.Scripts.Crosshairs;
using Assets.Scripts.Destruction;
using Assets.Scripts.GameStateMachine.CannonControl;
using Assets.Scripts.Input;
using Assets.Scripts.Spawn.Factories;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class CannonInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputSystem_Actions.CannonActions>().AsSingle();
            Container.BindInterfacesAndSelfTo<CannonInput>().AsSingle();

            Container.BindInterfacesAndSelfTo<CannonFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<CannonProjectionFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<CannonConstructionFactory>().AsSingle();

            Container.BindInterfacesAndSelfTo<ActiveCannonControllerContainer>().AsSingle();
            Container.BindInterfacesAndSelfTo<CannonInputProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<CannonExit>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<CannonDestructionHandler>().AsSingle();

            Container.BindInterfacesAndSelfTo<CannonCrosshair>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<FirstPersonCannonCrosshairPreview>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<ThirdPersonCannonCrosshairPreview>().FromComponentInHierarchy().AsSingle();
        }
    }
}