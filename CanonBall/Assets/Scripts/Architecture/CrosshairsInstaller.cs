using Assets.Scripts.Systems;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class CrosshairsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CrosshairSystem>().AsSingle();
        }
    }
}