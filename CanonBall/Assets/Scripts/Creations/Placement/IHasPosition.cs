using UnityEngine;

namespace Assets.Scripts.Creations.Placement
{
    public interface IHasPosition : IComponent
    {
        public Vector3 Position { get; }
    }
}
