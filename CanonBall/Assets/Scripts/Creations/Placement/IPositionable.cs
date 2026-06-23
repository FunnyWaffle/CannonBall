using UnityEngine;

namespace Assets.Scripts.Creations.Placement
{
    public interface IPositionable : IComponent
    {
        public void SetPosition(Vector3 position);
    }
}
