using Assets.Scripts.Camera;
using Assets.Scripts.Config;
using Assets.Scripts.Systems;
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
        [SerializeField] private SerializableDictionary<CameraViewType, CameraTransformPreset> _cameraViewPresets;

        [Header("Hud")]
        [SerializeField] private RectTransform _interactionPrompt;
        [SerializeField] private RectTransform _crosshair;

        private CameraPresetHandler _cameraPresetHandler;

        public CameraTransformPreset CameraPreset => _cameraPresetHandler.CurrentPreset;

        public void Initialize()
        {
            _cameraPresetHandler = new CameraPresetHandler(_cameraViewPresets);
        }

        public void SetCameraViewType(CameraViewType cameraViewType)
        {
            _cameraPresetHandler.SetViewType(cameraViewType);
        }

        public void SetCameraPivotRotation(Quaternion rotation)
        {
            _cameraPresetHandler.CurrentPreset.Pivot.rotation = rotation;
            RotateBody();
        }

        public void ShowInteractionPrompt()
        {
            _interactionPrompt.gameObject.SetActive(true);
        }

        public void HideInteractionPrompt()
        {
            _interactionPrompt.gameObject.SetActive(false);
        }

        public void Move(Vector3 velocity)
        {
            _characterController.Move(velocity * Time.deltaTime);

            var localVelocity = _model.InverseTransformDirection(velocity);
            _animator.SetFloat(SoldierAnimatorParameters.ForwardSpeed, localVelocity.z);
            _animator.SetFloat(SoldierAnimatorParameters.SideSpeed, localVelocity.x);
        }

        public void ShowCrosshair()
        {
            _crosshair.gameObject.SetActive(true);
        }

        public void HideCrosshair()
        {
            _crosshair.gameObject.SetActive(false);
        }

        private void RotateBody()
        {
            var flatDirection = Vector3.ProjectOnPlane(CameraSystem.MainCamera.Forward, Vector3.up);
            _model.rotation = Quaternion.LookRotation(flatDirection, Vector3.up);
        }
    }
}
