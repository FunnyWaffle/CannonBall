using Assets.Scripts.Camera;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class CameraSystem
    {
        private readonly MainCamera _mainCamera;
        private CameraPresetHandler _presetHandler;

        private ViewType _viewType = ViewType.FirstPerson;

        public CameraSystem(MainCamera mainCamera)
        {
            _mainCamera = mainCamera;
            _mainCamera.Initialize();
        }

        public MainCamera MainCamera => _mainCamera;

        public void RotateCameraPivot(Quaternion rotation)
        {
            if (_presetHandler == null)
                return;

            var preset = _presetHandler.GetPreset(_viewType);
            preset.Pivot.rotation = rotation;
        }

        public void ApplyMainCameraPreset(CameraPresetHandler presetHandler)
        {
            _presetHandler = presetHandler;
            SetPreset();
        }

        public void ChangeCameraViewType(ViewType viewType)
        {
            _viewType = viewType;
            SetPreset();
        }

        public bool TryGetMainCameraFacedCollider(out Collider collider, int ignoreLayer = 0)
        {
            return _mainCamera.TryGetFacedCollider(out collider, ignoreLayer);
        }

        public Vector3 ProjectOnMainCamera(Vector3 position)
        {
            return _mainCamera.WorldToScreenPoint(position);
        }

        private void SetPreset()
        {
            var preset = _presetHandler.GetPreset(_viewType);

            _mainCamera.SetPosition(preset.Position);
            _mainCamera.SetParent(preset.Pivot);
            _mainCamera.SetRotation(preset.Pivot.rotation);
        }
    }
}
