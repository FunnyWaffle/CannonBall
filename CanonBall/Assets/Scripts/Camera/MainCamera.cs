using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;

        private Transform _cameraTransform;

        public Vector3 Position => _cameraTransform.position;
        public Vector3 Forward => _cameraTransform.forward;
        public Vector3 Right => _cameraTransform.right;

        public void Initialize()
        {
            _cameraTransform = _camera.transform;
        }

        public void SetParent(Transform parent, bool worldPositionStays = true)
        {
            _cameraTransform.SetParent(parent, worldPositionStays);
        }

        public void SetPosition(Vector3 position)
        {
            _cameraTransform.position = position;
        }

        public void SetLocalPosition(Vector3 position)
        {
            _cameraTransform.localPosition = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            _cameraTransform.rotation = rotation;
        }

        public Vector3 WorldToScreenPoint(Vector3 position)
        {
            return _camera.WorldToScreenPoint(position);
        }

        public Vector3 GetFacedPosition(QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Collide,
            float maxDistance = float.MaxValue, int ignoreLayer = 0)
        {
            Debug.DrawRay(Position, Forward * maxDistance, Color.red, 0.1f);
            if (Physics.Raycast(Position, Forward, out var hit, maxDistance, ~ignoreLayer, queryTriggerInteraction))
                return hit.point;
            else
                return Position + Forward * 10f;
        }

        public bool TryGetFacedCollider(out Collider collider, float maxDistance = float.MaxValue, int ignoreLayer = 0)
        {
            if (Physics.Raycast(Position, Forward, out var hit, maxDistance, ~ignoreLayer))
            {
                collider = hit.collider;
                return true;
            }
            else
            {
                collider = null;
                return false;
            }
        }
    }
}