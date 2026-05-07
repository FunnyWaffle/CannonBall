using UnityEngine;

namespace Assets.Scripts.Spawn
{
    public interface ISpawnable
    {
        public void Place(Vector3 position, Quaternion rotation, Transform parent = null);
    }
}
