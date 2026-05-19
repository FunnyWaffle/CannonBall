using Assets.Scripts.Camera;
using Assets.Scripts.Systems;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Crosshairs
{
    public class CannonCrosshair : MonoBehaviour, ICrosshair
    {
        private readonly Dictionary<ViewType, ICrosshairPreview> _crosshairPreviews = new();

        private CameraSystem _cameraSystem;

        private ICrosshairPreview _crosshairPreview;

        [Inject]
        public void Initialize(CameraSystem cameraSystem, params ICrosshairPreview[] crosshairPreviews)
        {
            _cameraSystem = cameraSystem;

            foreach (var crosshair in crosshairPreviews)
            {
                _crosshairPreviews[crosshair.ViewType] = crosshair;
            }

            _crosshairPreview = _crosshairPreviews[ViewType.FirstPerson];
        }

        public CrosshairTypes CrosshairType => CrosshairTypes.Cannon;

        public void SwitchMode(ViewType viewType)
        {
            _crosshairPreview.SetActive(false);
            _crosshairPreview = _crosshairPreviews[viewType];
            _crosshairPreview.SetActive(true);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
            _crosshairPreview.SetActive(isActive);
        }

        public void SetPreviewPosition(Vector3 position)
        {
            if (_crosshairPreview.ViewType == ViewType.FirstPerson)
            {
                var screenPosition = _cameraSystem.ProjectOnMainCamera(position);
                if (!IsPositionBehindScreen(screenPosition.z))
                    _crosshairPreview.SetPosition(screenPosition);
            }
            else
            {
                _crosshairPreview.SetPosition(position);
            }
        }

        private bool IsPositionBehindScreen(float axisCoordinates)
        {
            return axisCoordinates < 0;
        }
    }
}
