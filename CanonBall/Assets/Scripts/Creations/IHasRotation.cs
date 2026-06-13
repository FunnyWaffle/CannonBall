using UnityEngine;

namespace Assets.Scripts.Creations
{
    public interface IHasRotation : IComponent
    {
        public Quaternion Rotation { get; }
    }
}
