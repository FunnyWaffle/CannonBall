using UnityEngine;

namespace Assets.Scripts.Spawn
{
    public class Spawner : MonoBehaviour
    {
        private readonly ObjectPool _objectPool = new();

        public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null)
            where T : MonoBehaviour, IPoolableObject
        {
            if (_objectPool.TryGet<T>(out var obj))
            {
                obj.gameObject.SetActive(true);
                obj.transform.SetLocalPositionAndRotation(position, rotation);
            }
            else
            {
                obj = Instantiate(prefab, position, rotation, parent);
                _objectPool.Register(obj);
            }

            return obj;
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
            => Instantiate(prefab, position, rotation, parent);
    }
}
