using Assets.Scripts.Systems;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Guns
{
    public class FirstPersonCannonCrosshairPreview : MonoBehaviour
    {
        [SerializeField] private RectTransform _firstPersonCrosshairPreview;

        [Inject] private CameraSystem _cameraSystem;

        public void SetPosition(Vector3 position)
        {
            var screenPosition = _cameraSystem.ProjectOnMainCamera(position);
            _firstPersonCrosshairPreview.position = screenPosition;
        }

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);
    }
}
