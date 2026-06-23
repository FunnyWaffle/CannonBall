using UnityEngine;

namespace Assets.Scripts.Creations.Placement
{
    public interface IPlaceable : IPositionable, IRotateable, IParentable
    {
        public void Place(Vector3 position, Quaternion rotation, Transform parent = null);
    }
}
