using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Assets.Scripts.Player.Cannon
{
    public class CannonRotator : MonoBehaviour
    {
        [SerializeField] private Transform _barrel;
        [SerializeField] private float _rotationSpeed = 1f;
        [SerializeField] private float _pitchAngleLimit = 15;

        [SerializeField] private RectTransform _firstPersonCrosshairPreview;
        [SerializeField] private DecalProjector _thirdPersonCrosshairPreview;

        [Inject] private Player _player;

        private void Start()
        {
            _player.ViewModeChanged += OnPlayerViewModeChanged;
        }

        private void Update()
        {
            var playerCamera = _player.Camera;
            Vector3 targetPosition;

            targetPosition = GetTargetPosition(playerCamera);
            Quaternion targetRotationQuaternion = GetTargetDirection(targetPosition);

            _barrel.localRotation = Quaternion.RotateTowards(
                _barrel.rotation,
                targetRotationQuaternion,
                _rotationSpeed * Time.deltaTime);

            MoveCrosshairPreview(playerCamera);
        }

        private Vector3 GetTargetPosition(PlayerCamera playerCamera)
        {
            Vector3 cameraForward = playerCamera.Forward;
            Vector3 cameraPosition = playerCamera.Position;

            Vector3 targetPosition;
            if (Physics.Raycast(cameraPosition, cameraForward, out var hit, float.PositiveInfinity))
            {
                targetPosition = hit.point;
            }
            else
            {
                targetPosition = cameraPosition + cameraForward * 10f;
            }

            return targetPosition;
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

        private void MoveCrosshairPreview(PlayerCamera playerCamera)
        {
            Vector3 position;
            if (Physics.Raycast(_barrel.position, _barrel.forward, out var hit, float.PositiveInfinity))
            {
                position = hit.point;
            }
            else
            {
                position = _barrel.position + _barrel.forward * 10f;
            }

            var screenPosition = playerCamera.WorldToScreenPoint(position);
            _firstPersonCrosshairPreview.position = screenPosition;

            _thirdPersonCrosshairPreview.transform.position = position + Vector3.up
                * (_thirdPersonCrosshairPreview.size.y - 0.01f);
        }

        private void OnPlayerViewModeChanged(int viewModeIndex)
        {
            switch (viewModeIndex)
            {
                case 0:
                    _firstPersonCrosshairPreview.gameObject.SetActive(true);
                    _thirdPersonCrosshairPreview.gameObject.SetActive(false);
                    break;
                case 1:
                    _firstPersonCrosshairPreview.gameObject.SetActive(false);
                    _thirdPersonCrosshairPreview.gameObject.SetActive(true);
                    break;
            }
        }
    }
}
