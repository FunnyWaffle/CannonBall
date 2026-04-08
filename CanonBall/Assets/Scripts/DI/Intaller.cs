using Assets.Scripts.Player;
using Assets.Scripts.Player.Cannon;
using Assets.Scripts.Player.Cannon.Projectile;
using Assets.Scripts.Spawn;
using UnityEngine;
using Zenject;

public class Intaller : MonoInstaller
{
    [SerializeField] private MonoBehaviour[] _dependencies;
    public override void InstallBindings()
    {
        //foreach (var dependency in _dependencies)
        //{
        //    Container.BindInterfacesAndSelfTo(dependency.GetType()).FromInstance(dependency).AsSingle();

        //}

        Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<ObjectPool<Ball>>().AsSingle();

        Container.BindInterfacesAndSelfTo<Spawner>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<CannonShoot>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<CannonMover>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAimer>().FromComponentInHierarchy().AsSingle();

    }
}