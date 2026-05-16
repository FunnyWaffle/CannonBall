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

        public void ApplyMainCameraPreset(CameraPresetHandler presetHandler)
        {
            _presetHandler = presetHandler;

        }

        public void ChangeCameraViewType(int viewModeIndex)
        {
            var cameraViewType = viewModeIndex switch
            {
                0 => CameraViewType.FirstPerson,
                1 => CameraViewType.ThirdPerson,
                _ => throw new System.NotImplementedException(),
            };

            var preset = _presetHandler.GetPreset(cameraViewType);

            _mainCamera.SetPosition(preset.Position);
            _mainCamera.SetParent(preset.Pivot);
            _mainCamera.SetRotation(preset.Pivot.rotation);
        }

        public bool TryGetMainCameraFacedCollider(out Collider collider, int ignoreLayer = ~0)
        {
            return _mainCamera.TryGetFacedCollider(out collider, ignoreLayer);
        }

        public Vector3 ProjectOnMainCamera(Vector3 position)
        {
            return _mainCamera.WorldToScreenPoint(position);
        }
    }
}
