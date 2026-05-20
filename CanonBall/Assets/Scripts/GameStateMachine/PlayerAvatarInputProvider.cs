using Assets.Scripts.Config;
using Assets.Scripts.Creations;
using Assets.Scripts.Input;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.GameStateMachine
{
    public class PlayerAvatarInputProvider : IUpdatable
    {
        private readonly PlayerAvatarInput _input;
        private readonly Aimer _aimer;
        private readonly CameraSystem _cameraSystem;
        private readonly CrosshairSystem _crosshairSystem;

        private IPlayerAvatarController _controller;

        public PlayerAvatarInputProvider(
            PlayerAvatarInput input,
            Aimer aimer,
            CameraSystem cameraSystem,
            CrosshairSystem crosshairSystem,
            IPlayerAvatarController controller)
        {
            _input = input;
            _aimer = aimer;
            _cameraSystem = cameraSystem;
            _crosshairSystem = crosshairSystem;

            PrivateSet(controller);
            _cameraSystem.ChangeCameraViewType(Camera.ViewType.FirstPerson);

            _input.AttackActionPerformed += OnAttack;
        }

        public void SetController(IPlayerAvatarController controller)
        {
            _controller.Stop();
            PrivateSet(controller);
        }

        public void Update()
        {
            var lookInput = _input.Look;
            var rotation = _aimer.Aim(lookInput);

            _cameraSystem.RotateCameraPivot(rotation);
            var position = _cameraSystem.MainCamera.GetFacedPosition(QueryTriggerInteraction.Ignore, LayerIds.BitMaskPlayer);

            _controller.Rotate(position);
            var movementInput = _input.Movement;
            _controller.Move(movementInput);
        }

        public void OnAttack()
        {
            _controller.Attack();
        }

        private void PrivateSet(IPlayerAvatarController controller)
        {
            var preset = controller.GetCameraTransformPreset();
            _cameraSystem.ApplyMainCameraPreset(preset);
            _crosshairSystem.EnableCrosshair(CrosshairTypes.Player);
            _controller = controller;
        }
    }
}
