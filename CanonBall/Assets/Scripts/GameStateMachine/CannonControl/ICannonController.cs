using Assets.Scripts.Camera;
using Assets.Scripts.Creations;
using UnityEngine;

namespace Assets.Scripts.GameStateMachine.CannonControl
{
    public interface ICannonController : IComponent
    {
        public void Rotate(Vector3 positionToRotation);
        public void Shoot();
        public CameraPresetHandler GetCameraTransformPreset();
    }
}
