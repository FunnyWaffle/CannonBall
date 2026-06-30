using UnityEngine;

namespace Assets.Scripts.Creations
{
    public interface IHasSize : IComponent
    {
        public Vector3 Size { get; }
    }
}
