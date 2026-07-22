using Assets.Scripts.Combat;
using Assets.Scripts.Explosion;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class CombatInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DamageSystem>().AsSingle();

            Container.BindInterfacesAndSelfTo<ExplosionHandler>().AsSingle();
        }
    }
}