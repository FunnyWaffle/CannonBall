using Assets.Scripts.Creations;

namespace Assets.Scripts.Combat
{
    public interface IHasHealth : IComponent
    {
        public float Health { get; }
    }
}
