using Assets.Scripts.Creations;

namespace Assets.Scripts.Guns.Projections
{
    public interface IConstructionProjection : IComponent
    {
        public float BuildCarriageOffset { get; }
    }
}
