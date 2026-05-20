using Assets.Scripts.Camera;
using UnityEngine;

namespace Assets.Scripts.GameStateMachine
{
    public interface ICannonController
    {
        public void Rotate(Vector3 positionToRotation);
        public void Shoot();
        public CameraPresetHandler GetCameraTransformPreset();
    }
}
