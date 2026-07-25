using Assets.Scripts.GameStateMachine.UIOverlayControl;
using Assets.Scripts.GameStateMachine.UIWindowsControl;
using Assets.Scripts.UI;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UIController>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIOverlayController>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameOverMenu>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<InteractionPrompt>().FromComponentInHierarchy().AsSingle();
        }
    }
}