using Assets.Scripts.Camera;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class CameraSystem
    {
        private readonly MainCamera _mainCamera;
        private CameraPresetHandler _presetHandler;

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

            _presetHandler.SetCurrentPresetRotation(rotation);
        }

        public void ApplyMainCameraPreset(CameraPresetHandler presetHandler)
        {
            _presetHandler = presetHandler;
            SetPreset();
        }

        public void ChangeCameraViewType(ViewType viewType)
        {
            _presetHandler.SetViewType(viewType);
            SetPreset();
        }

        public bool TryGetMainCameraFacedCollider(out Collider collider, float maxDistance = float.MaxValue, int ignoreLayer = 0)
        {
            return _mainCamera.TryGetFacedCollider(out collider, maxDistance, ignoreLayer);
        }

        public Vector3 TryGetMainCameraFacedPosition(QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Collide,
            float maxDistance = float.MaxValue, int ignoreLayer = 0)
        {
            return _mainCamera.GetFacedPosition(queryTriggerInteraction, maxDistance, ignoreLayer);
        }

        public Vector3 ProjectOnMainCamera(Vector3 position)
        {
            return _mainCamera.WorldToScreenPoint(position);
        }

        private void SetPreset()
        {
            var preset = _presetHandler.CurrentPreset;

            _mainCamera.SetParent(preset.Slot, true);
            _mainCamera.SetLocalPosition(Vector3.zero);
            _mainCamera.SetRotation(preset.Slot.rotation);
        }
    }
}
