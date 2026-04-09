using UnityEngine;

namespace Assets.Scripts.Guns
{
    public class CannonRotator : MonoBehaviour
    {
        [SerializeField] private Transform _barrel;
        [SerializeField] private float _rotationSpeed = 1f;
        [SerializeField] private float _pitchAngleLimit = 15;

        public void RotateToPosition(Vector3 targetPosition)
        {
            Quaternion targetRotationQuaternion = GetTargetDirection(targetPosition);

            _barrel.localRotation = Quaternion.RotateTowards(
                _barrel.rotation,
                targetRotationQuaternion,
                _rotationSpeed * Time.deltaTime);
        }

        private Quaternion GetTargetDirection(Vector3 targetPosition)
        {
            var targetDirection = Vector3.Normalize(targetPosition - _barrel.position);
            var targetRotation = Quaternion.LookRotation(targetDirection).eulerAngles;

            var pitch = targetRotation.x;
            if (pitch > 180f)
                pitch -= 360;

            var clampedPitch = Mathf.Clamp(pitch, -_pitchAngleLimit, _pitchAngleLimit);

            return Quaternion.Euler(clampedPitch, targetRotation.y, 0f);
        }
    }
}
