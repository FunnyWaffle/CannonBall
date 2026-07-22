using Assets.Scripts.Systems;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class NavigationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CustomPathFinder>().AsSingle();
        }
    }
}