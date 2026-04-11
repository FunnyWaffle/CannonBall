namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieCore
    {
        private readonly ZombieMover _mover;
        private readonly ZombieRagdoll _ragdoll;

        public ZombieCore(ZombieMover mover, ZombieRagdoll ragdoll)
        {
            _mover = mover;
            _ragdoll = ragdoll;
        }
    }
}
