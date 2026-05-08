using UnityEngine;

namespace Assets.Scripts.Spawn.Factories
{
    public interface IFactory<T>
    {
        public T Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null);
    }
}
