using Assets.Scripts.Camera;
using Assets.Scripts.Guns.Projectile;
using Assets.Scripts.Wrappers;
using System;
using UnityEngine;

namespace Assets.Scripts.Guns
{
    public class CannonView : MonoBehaviour
    {
        public Collider[] Colliders;

        [Header("Rotation")]
        [SerializeField] private Transform _barrel;
        [SerializeField] private float _rotationSpeed = 1f;
        [SerializeField] private float _pitchAngleLimit = 15;

        [Header("Shoot")]
        [SerializeField] private Ball _projectile;
        [SerializeField] private Transform _barrelExit;
        [SerializeField] private float _shootPower = 15f;
        [SerializeField] private float _shootDelay = 1.5f;

        [Header("Crosshairs")]
        [SerializeField] private FirstPersonCannonCrosshairPreview _firstPersonCrosshairPreview;
        [SerializeField] private ThirdPersonCannonCrosshairPreview _thirdPersonCrosshairPreview;
        [SerializeField] private RectTransform _playerCrosshair;

        [Header("Camera")]
        [SerializeField] private SerializableDictionary<CameraViewType, CameraTransformPreset> _cameraViewPresets;

        private CameraPresetHandler _cameraPresetHandler;

        public CameraTransformPreset CameraPreset => _cameraPresetHandler.CurrentPreset;

        public Quaternion BarrelLocalRotation => _barrel.localRotation;
        public float RotationSpeed => _rotationSpeed;
        public float PitchAngleLimit => _pitchAngleLimit;

        public Quaternion BarrelExitRotation => _barrelExit.rotation;
        public Vector3 BarrelExitForward => _barrelExit.forward;
        public Vector3 BarrelExitPosition => _barrelExit.position;
        public float ShootPower => _shootPower;
        public float ShootDelay => _shootDelay;
        public Ball Projectile => _projectile;

        public FirstPersonCannonCrosshairPreview FirstPersonCrosshairPreview => _firstPersonCrosshairPreview;
        public ThirdPersonCannonCrosshairPreview ThirdPersonCrosshairPreview => _thirdPersonCrosshairPreview;

        public event Action<float> RotationSpeedChanged;
        public event Action<float> PitchLimitChanged;

        public event Action<float> ShootPowerChanged;
        public event Action<float> ShootDelayChanged;

        public void Initialize()
        {
            _cameraPresetHandler = new CameraPresetHandler(_cameraViewPresets);
        }

        public void SetCameraViewType(CameraViewType cameraViewType)
        {
            _cameraPresetHandler.SetViewType(cameraViewType);
        }

        public void SetBarrelRotation(Quaternion rotation)
        {
            _barrel.localRotation = rotation;
        }

        public void SetCameraPivotRotation(Quaternion rotation)
        {
            _cameraPresetHandler.CurrentPreset.Pivot.rotation = rotation;
        }

        public void ShowCrosshair(CrosshairMode crosshairMode)
        {
            _playerCrosshair.gameObject.SetActive(true);
            if (crosshairMode == CrosshairMode.FirstPerson)
            {
                FirstPersonCrosshairPreview.SetActive(true);
                ThirdPersonCrosshairPreview.SetActive(false);
            }
            else
            {
                FirstPersonCrosshairPreview.SetActive(false);
                ThirdPersonCrosshairPreview.SetActive(true);
            }
        }

        public void HideCrosshair()
        {
            _playerCrosshair.gameObject.SetActive(true);
        }

        private void OnValidate()
        {
            RotationSpeedChanged?.Invoke(_rotationSpeed);
            PitchLimitChanged?.Invoke(_pitchAngleLimit);

            ShootPowerChanged?.Invoke(_shootPower);
            ShootDelayChanged?.Invoke(_shootDelay);
        }
    }
}
