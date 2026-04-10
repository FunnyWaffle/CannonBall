using UnityEngine;

namespace Assets.Scripts.Guns
{
    public class CannonRotator : MonoBehaviour
    {
        [SerializeField] private Transform _barrel;
        [SerializeField] private float _rotationSpeed = 1f;
        [SerializeField] private float _pitchAngleLimit = 15;

        public void RotateInDirection(Vector3 direction)
        {
            Quaternion targetRotationQuaternion = ClampDirection(direction);

            _barrel.localRotation = Quaternion.RotateTowards(
                _barrel.localRotation,
                targetRotationQuaternion,
                _rotationSpeed * Time.deltaTime);
        }

        private Quaternion ClampDirection(Vector3 targetPosition)
        {
            var targetRotation = Quaternion.LookRotation(targetPosition).eulerAngles;

            var pitch = targetRotation.x;
            if (pitch > 180f)
                pitch -= 360;

            var clampedPitch = Mathf.Clamp(pitch, -_pitchAngleLimit, _pitchAngleLimit);

            return Quaternion.Euler(clampedPitch, targetRotation.y, 0f);
        }
    }
}
