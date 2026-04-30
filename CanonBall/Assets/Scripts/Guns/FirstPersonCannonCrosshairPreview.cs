using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.Guns
{
    public class FirstPersonCannonCrosshairPreview : MonoBehaviour
    {
        [SerializeField] private RectTransform _firstPersonCrosshairPreview;

        public void SetPosition(Vector3 position)
        {
            var screenPosition = CameraSystem.ProjectOnMainCamera(position);
            if (IsPositionBehindScreen(screenPosition.z))
            {
                SetActive(false);
                return;
            }

            SetActive(true);
            _firstPersonCrosshairPreview.position = screenPosition;
        }

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);

        private bool IsPositionBehindScreen(float axisCoordinates)
        {
            return axisCoordinates < 0;
        }
    }
}
