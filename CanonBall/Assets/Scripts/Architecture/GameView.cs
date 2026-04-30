using Assets.Scripts.Architecture;
using Assets.Scripts.Camera;
using Assets.Scripts.Creations.Player;
using Assets.Scripts.Guns;
using Assets.Scripts.PlayerData;
using Assets.Scripts.Shop;
using Assets.Scripts.Systems;
using UnityEngine;
using Zenject;

public class GameView : MonoInstaller
{
    [SerializeField] private MonoBehaviour[] _dependencies;
    public override void InstallBindings()
    {
        //foreach (var dependency in _dependencies)
        //{
        //    Container.BindInterfacesAndSelfTo(dependency.GetType()).FromInstance(dependency).AsSingle();

        //}
        Container.BindInterfacesAndSelfTo<PlayerAvatarView>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<CannonView>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<MainCamera>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<InventoryView>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<Shop>().FromComponentInHierarchy().AsSingle();

        var mainCamera = Container.Resolve<MainCamera>();
        CameraSystem.SetMainCamera(mainCamera);

        new GameCore(Container);
        new GameController(Container);
    }
}