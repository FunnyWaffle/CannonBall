using Assets.Scripts.Camera;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.Input
{
    public interface IController
    {
        public CrosshairTypes CrosshairType { get; }

        public void HandleInput(Vector2 movementInput, Vector3 positionToRotation);
        public CameraPresetHandler GetCameraTransformPreset();
    }
}
