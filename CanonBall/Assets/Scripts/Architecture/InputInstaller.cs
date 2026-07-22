using Assets.Scripts.Input;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class InputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputSystem_Actions>().AsSingle();

            Container.BindInterfacesAndSelfTo<InputSystem>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<Aimer>().AsSingle();
        }
    }
}