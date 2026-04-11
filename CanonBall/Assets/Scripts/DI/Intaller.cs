using Assets.Scripts;
using Assets.Scripts.Creations.Player;
using Assets.Scripts.Creations.Zombie;
using Assets.Scripts.Guns;
using Assets.Scripts.Guns.Projectile;
using Assets.Scripts.Input;
using Assets.Scripts.Spawn;
using Assets.Scripts.Systems;
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

        Container.BindInterfacesAndSelfTo<Spawner>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<Updater>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<CameraSystem>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo<CannonShooter>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<CannonRotator>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<FirstPersonCannonCrosshairPreview>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<ThirdPersonCannonCrosshairPreview>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo<PlayerAimer>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerCamera>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo<ZombieRagdoll>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<ZombieMover>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<ObjectPool<Ball>>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerCore>().AsSingle();
        Container.BindInterfacesAndSelfTo<ZombieCore>().AsSingle();
        Container.BindInterfacesAndSelfTo<Cannon>().AsSingle();

        Container.Resolve<PlayerCore>();
    }
}