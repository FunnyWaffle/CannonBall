using UnityEngine;

namespace Assets.Scripts.Creations.Placement
{
    public interface IHasRotation : IComponent
    {
        public Quaternion Rotation { get; }
    }
}
