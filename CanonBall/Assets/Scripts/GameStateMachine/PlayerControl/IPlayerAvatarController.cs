using Assets.Scripts.Camera;
using UnityEngine;

namespace Assets.Scripts.GameStateMachine.PlayerControl
{
    public interface IPlayerAvatarController
    {
        public void Move(Vector2 movementInput);
        public void Rotate(Vector3 positionToRotation);
        public void Attack();
        public CameraPresetHandler GetCameraTransformPreset();
        public void Stop();
        public void Jump();
    }
}
