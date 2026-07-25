using Assets.Scripts.Curency;
using Assets.Scripts.GameStateMachine.UIControl;
using Assets.Scripts.Input;
using Assets.Scripts.Shop;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class ShopInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputSystem_Actions.ShopActions>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShopInput>().AsSingle();

            Container.BindInterfacesAndSelfTo<ShopInputProvider>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<Vendor>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<WaveCurrencyAccruer>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<PurchaseHandler>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<ShopView>().FromComponentInHierarchy().AsSingle();
        }
    }
}