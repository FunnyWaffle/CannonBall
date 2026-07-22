using Assets.Scripts.Camera;
using Assets.Scripts.Systems;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class CamerasInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MainCamera>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraSystem>().AsSingle();
        }
    }
}