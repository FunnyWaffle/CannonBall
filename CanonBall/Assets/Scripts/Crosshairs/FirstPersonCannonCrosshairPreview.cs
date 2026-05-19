using Assets.Scripts.Camera;
using UnityEngine;

namespace Assets.Scripts.Crosshairs
{
    public class FirstPersonCannonCrosshairPreview : MonoBehaviour, ICrosshairPreview
    {
        [SerializeField] private RectTransform _firstPersonCrosshairPreview;

        public ViewType ViewType => ViewType.FirstPerson;

        public void SetPosition(Vector3 position)
        {
            _firstPersonCrosshairPreview.position = position;
        }

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);
    }
}
