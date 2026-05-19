using Assets.Scripts.Camera;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.Input
{
    public interface IController
    {
        public CrosshairTypes CrosshairType { get; }

        public void Move(Vector2 movementInput);
        public void Rotate(Vector3 positionToRotation);
        public void Attack();
        public CameraPresetHandler GetCameraTransformPreset();
    }
}
