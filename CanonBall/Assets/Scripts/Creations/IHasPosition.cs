using UnityEngine;

namespace Assets.Scripts.Creations
{
    public interface IHasPosition : IComponent
    {
        public Vector3 Position { get; }
    }
}
