using Assets.Scripts.Camera;
using Assets.Scripts.Config;
using Assets.Scripts.Wrappers;
using UnityEngine;

namespace Assets.Scripts.Creations.Player
{
    public class PlayerAvatarView : MonoBehaviour
    {
        [Header("Model")]
        [SerializeField] private Transform _model;
        [Header("Movement")]
        [SerializeField] private CharacterController _characterController;
        [Header("Animator")]
        [SerializeField] private Animator _animator;

        [Header("Camera")]
        [SerializeField] private SerializableDictionary<ViewType, CameraTransformPreset> _cameraViewPresets;

        public CameraPresetHandler CameraPresetHandler { get; private set; }
        public Vector3 ModelPosition => _model.position;
        public bool IsGrounded => _characterController.isGrounded;

        public void Initialize()
        {
            CameraPresetHandler = new CameraPresetHandler(_cameraViewPresets);
        }

        public void SetCameraPivotRotation(Quaternion rotation)
        {
            RotateBody(rotation);
        }

        public void Move(Vector3 velocity)
        {
            _characterController.Move(velocity * Time.deltaTime);

            var localVelocity = _model.InverseTransformDirection(velocity);
            SetAnimationVelocity(localVelocity);
        }

        public void Stop()
        {
            SetAnimationVelocity(Vector3.zero);
        }

        public void EnableJumpAnimation()
        {
            _animator.SetTrigger(SoldierAnimatorParameters.Jumped);
        }

        private void RotateBody(Quaternion rotation)
        {
            _model.rotation = rotation;
        }

        private void SetAnimationVelocity(Vector3 localVelocity)
        {
            _animator.SetFloat(SoldierAnimatorParameters.ForwardSpeed, localVelocity.z);
            _animator.SetFloat(SoldierAnimatorParameters.SideSpeed, localVelocity.x);
        }
    }
}
