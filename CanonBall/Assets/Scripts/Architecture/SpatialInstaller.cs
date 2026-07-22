using Assets.Scripts.Space;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class SpatialInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<World>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpatialSearchShape>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpatialGrid>().FromComponentInHierarchy().AsSingle();
        }
    }
}