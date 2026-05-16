using UnityEngine;
using Zenject;

namespace Assets.Scripts.Crosshairs
{
    public class CannonCrosshair : MonoBehaviour
    {
        [Inject] private FirstPersonCannonCrosshairPreview _firstPersonCannonCrosshairPreview;
        [Inject] private ThirdPersonCannonCrosshairPreview _thirdPersonCrosshairPreview;

        public void SwitchMode(CrosshairMode crosshairMode)
        {
            SetActive(true);
            if (crosshairMode == CrosshairMode.FirstPerson)
            {
                _firstPersonCannonCrosshairPreview.SetActive(true);
                _thirdPersonCrosshairPreview.SetActive(false);
            }
            else
            {
                _firstPersonCannonCrosshairPreview.SetActive(false);
                _thirdPersonCrosshairPreview.SetActive(true);
            }
        }

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);

        public void SetPreviewPosition(Vector3 position)
        {
            _firstPersonCannonCrosshairPreview.SetPosition(position);
            _thirdPersonCrosshairPreview.SetPosition(position);
        }
    }
}
