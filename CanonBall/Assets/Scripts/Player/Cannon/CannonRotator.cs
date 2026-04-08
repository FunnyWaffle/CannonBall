using UnityEngine;

namespace Assets.Scripts.Player.Cannon
{
    public class CannonRotator : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _barrel;
        [SerializeField] private float _rotationSpeed = 1f;
        [SerializeField] private float _pitchAngleLimit = 15;

        private void Update()
        {
            var targetRotation = _camera.transform.eulerAngles;

            var pitch = targetRotation.x;
            if (pitch > 180f)
                pitch -= 360;

            var clampedPitch = Mathf.Clamp(pitch, -_pitchAngleLimit, _pitchAngleLimit);

            var targetRotationQuaternion = Quaternion.Euler(clampedPitch, targetRotation.y, 0f);

            _barrel.localRotation = Quaternion.RotateTowards(
                _barrel.rotation,
                targetRotationQuaternion,
                _rotationSpeed * Time.deltaTime);
        }
    }
}
