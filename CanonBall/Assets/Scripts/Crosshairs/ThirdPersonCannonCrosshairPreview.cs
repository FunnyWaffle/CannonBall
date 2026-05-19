using Assets.Scripts.Camera;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.Crosshairs
{
    public class ThirdPersonCannonCrosshairPreview : MonoBehaviour, ICrosshairPreview
    {
        [SerializeField] private DecalProjector _crosshair;

        private Transform _transform;

        public ViewType ViewType => ViewType.ThirdPerson;

        private void Awake()
        {
            _transform = _crosshair.transform;
        }

        public void SetPosition(Vector3 position)
        {
            _transform.position = position + Vector3.up
                * (_crosshair.size.y - 0.01f);
        }

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);
    }
}
