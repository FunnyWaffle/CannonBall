using Assets.Scripts.Camera;
using Assets.Scripts.Config;
using Assets.Scripts.Creations.Placement;
using Assets.Scripts.Wrappers;
using System;
using UnityEngine;

namespace Assets.Scripts.Creations.Player
{
    public class PlayerAvatarView : MonoBehaviour, IHasPosition, IPositionChangeNotifier, IRotationChangeNotifier, IHasRotation
    {
        [Header("Model")]
        [SerializeField] private Transform _model;
        [Header("Movement")]
        [SerializeField] private CharacterController _characterController;
        [Header("Animator")]
        [SerializeField] private Animator _animator;
        [Header("Camera")]
        [SerializeField] private SerializableDictionary<ViewType, CameraTransformPreset> _cameraViewPresets;
        [Header("AttackCorners")]
        [SerializeField] private Transform[] _attackCorners;

        private Transform _transform;

        public CameraPresetHandler CameraPresetHandler { get; private set; }
        public Quaternion Rotation => _model.rotation;
        public Vector3 Position => _transform.position;
        public Vector3 ModelPosition => _model.position;
        public bool IsGrounded => _characterController.isGrounded;
        public Transform[] AttackCorners => _attackCorners;
        public Collider Collider => _characterController;

        public event Action<Vector3> PositionChanged;
        public event Action<Quaternion> RotationChanged;

        public void Initialize()
        {
            CameraPresetHandler = new CameraPresetHandler(_cameraViewPresets);
            _transform = _characterController.transform;
        }

        public void SetCameraPivotRotation(Quaternion rotation)
        {
            RotateBody(rotation);
        }

        public void Move(Vector3 velocity)
        {
            if (velocity != Vector3.zero)
            {
                _characterController.Move(velocity * Time.deltaTime);
                PositionChanged?.Invoke(Position);
            }

            var localVelocity = _model.InverseTransformDirection(velocity);
            SetAnimationVelocity(localVelocity);
        }

        public void Stop()
        {
            SetAnimationVelocity(Vector3.zero);
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
            _transform.position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            _transform.rotation = rotation;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void EnableJumpAnimation()
        {
            _animator.SetTrigger(SoldierAnimatorParameters.Jump);
        }

        private void RotateBody(Quaternion rotation)
        {
            _model.rotation = rotation;
            RotationChanged?.Invoke(rotation);
        }

        private void SetAnimationVelocity(Vector3 localVelocity)
        {
            _animator.SetFloat(SoldierAnimatorParameters.ForwardSpeed, localVelocity.z);
            _animator.SetFloat(SoldierAnimatorParameters.SideSpeed, localVelocity.x);
        }
    }
}
