using Assets.Scripts.Camera;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public static class CameraSystem
    {
        private static MainCamera _mainCamera;

        public static MainCamera MainCamera => _mainCamera;

        public static void SetMainCamera(MainCamera mainCamera)
        {
            _mainCamera = mainCamera;
            _mainCamera.Initialize();
        }

        public static void ApplyMainCameraPreset(CameraTransformPreset preset)
        {
            _mainCamera.SetPosition(preset.Position);
            _mainCamera.SetParent(preset.Pivot);
            _mainCamera.SetRotation(preset.Pivot.rotation);
        }

        public static bool TryGetMainCameraFacedCollider(out Collider collider, int ignoreLayer = ~0)
        {
            return _mainCamera.TryGetFacedCollider(out collider, ignoreLayer);
        }

        public static Vector3 ProjectOnMainCamera(Vector3 position)
        {
            return _mainCamera.WorldToScreenPoint(position);
        }
    }
}
