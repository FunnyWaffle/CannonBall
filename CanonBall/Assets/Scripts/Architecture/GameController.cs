using Assets.Scripts.Creations.Player;
using Assets.Scripts.Guns;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class GameController
    {
        public GameController(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<PlayerAvatarController>().AsSingle();
            container.BindInterfacesAndSelfTo<CannonController>().AsSingle();
        }
    }
}
