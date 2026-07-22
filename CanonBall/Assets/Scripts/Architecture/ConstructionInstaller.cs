using Assets.Scripts.Build;
using Assets.Scripts.Spawn.Factories;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class ConstructionInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BuildSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<BuilderFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<BuildCarriageFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<BuilderSpawnZone>().FromComponentInHierarchy().AsSingle();
        }
    }
}