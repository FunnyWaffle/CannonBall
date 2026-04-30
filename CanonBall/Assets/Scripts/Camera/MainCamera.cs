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

        public void SetParent(Transform parent)
        {
            _cameraTransform.SetParent(parent, true);
        }

        public void SetPosition(Vector3 position)
        {
            _cameraTransform.position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            _cameraTransform.rotation = rotation;
        }

        public Vector3 WorldToScreenPoint(Vector3 position)
        {
            return _camera.WorldToScreenPoint(position);
        }

        public Vector3 GetFacedPosition()
        {
            Debug.DrawRay(Position, Forward * float.PositiveInfinity, Color.red, 0.1f);
            if (Physics.Raycast(Position, Forward, out var hit, float.PositiveInfinity))
                return hit.point;
            else
                return Position + Forward * 10f;
        }

        public bool TryGetFacedCollider(out Collider collider, int ignoreLayer = ~0)
        {
            if (Physics.Raycast(Position, Forward, out var hit, float.PositiveInfinity, ~ignoreLayer))
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