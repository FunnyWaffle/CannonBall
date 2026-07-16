using Assets.Scripts.Creations.Placement;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creations.Builder
{
    public class BuilderView : MonoBehaviour, IPlaceable, IHasPosition
    {
        [SerializeField] private NavMeshAgent _agent;

        public NavMeshAgent Agent => _agent;
        public Vector3 Position => transform.position;

        public void Enable() => gameObject.SetActive(true);

        public void Place(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            SetPosition(position);
            Rotate(rotation);
            SetParent(parent);
        }

        public void Rotate(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
