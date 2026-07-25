using Assets.Scripts.GameStateMachine.UIControl;
using Assets.Scripts.Input;
using Assets.Scripts.PlayerData;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class InventoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputSystem_Actions.InventoryActions>().AsSingle();
            Container.BindInterfacesAndSelfTo<InventoryInput>().AsSingle();

            Container.BindInterfacesAndSelfTo<InventoryController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ItemAdder>().AsSingle();

            Container.BindInterfacesAndSelfTo<InventoryInputProvider>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<InventoryView>().FromComponentInHierarchy().AsSingle();
        }
    }
}