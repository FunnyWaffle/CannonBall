using Assets.Scripts.Interaction;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class InteractionInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InteractionObjectsRepositiory>().AsSingle();
        }
    }
}