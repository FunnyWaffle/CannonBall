using UnityEngine;

namespace Assets.Scripts.Creations.Placement
{
    public interface IParentable
    {
        public void SetParent(Transform parent);
    }
}
