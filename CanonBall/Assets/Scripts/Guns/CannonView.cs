using Assets.Scripts.Camera;
using Assets.Scripts.Wrappers;
using System;
using UnityEngine;

namespace Assets.Scripts.Guns
{
    public class CannonView : MonoBehaviour
    {

        [Header("Rotation")]
        [SerializeField] private Transform _barrel;
        [SerializeField] private float _rotationSpeed = 1f;
        [SerializeField] private float _pitchAngleLimit = 15;

        [Header("Shoot")]
        [SerializeField] private Transform _barrelExit;
        [SerializeField] private float _shootPower = 15f;
        [SerializeField] private float _shootDelay = 1.5f;

        [Header("Camera")]
        [SerializeField] private SerializableDictionary<ViewType, CameraTransformPreset> _cameraViewPresets;

        [Header("HitBox")]
        [SerializeField] private Collider[] _colliders;
        [SerializeField] private Transform[] _attackZoneCorners;

        public CameraPresetHandler CameraPresetHandler { get; private set; }

        public Vector3 Position => transform.position;

        public Quaternion BarrelLocalRotation => _barrel.localRotation;
        public float RotationSpeed => _rotationSpeed;
        public float PitchAngleLimit => _pitchAngleLimit;

        public Quaternion BarrelExitRotation => _barrelExit.rotation;
        public Vector3 BarrelExitForward => _barrelExit.forward;
        public Vector3 BarrelExitPosition => _barrelExit.position;
        public float ShootPower => _shootPower;
        public float ShootDelay => _shootDelay;

        public Collider[] Colliders => _colliders;
        public Transform[] AttackZoneCorners => _attackZoneCorners;

        public event Action<float> RotationSpeedChanged;
        public event Action<float> PitchLimitChanged;

        public event Action<float> ShootPowerChanged;
        public event Action<float> ShootDelayChanged;

        public void Initialize()
        {
            CameraPresetHandler = new CameraPresetHandler(_cameraViewPresets);
        }

        public void SetBarrelRotation(Quaternion rotation)
        {
            _barrel.localRotation = rotation;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
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
