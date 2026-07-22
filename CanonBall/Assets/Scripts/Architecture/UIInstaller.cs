using Assets.Scripts.GameStateMachine;
using Assets.Scripts.UI;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UIController>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameOverMenu>().FromComponentInHierarchy().AsSingle();
        }
    }
}