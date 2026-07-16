using UnityEngine;

namespace Assets.Scripts.Creations.Placement
{
    public interface IRotateable : IComponent
    {
        public void Rotate(Quaternion rotation);
    }
}
