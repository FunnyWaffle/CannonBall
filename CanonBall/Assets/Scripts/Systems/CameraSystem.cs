using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class CameraSystem : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;

        public Vector3 ProjectOnMainCamera(Vector3 position)
        {
            return _mainCamera.WorldToScreenPoint(position);
        }
    }
}
