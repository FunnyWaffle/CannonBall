using Assets.Scripts.Config;
using Assets.Scripts.Input;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.GameStateMachine
{
    public class GameplayController
    {
        private readonly CameraSystem _cameraSystem;
        private readonly Aimer _aimer;

        private IController _controller;

        public GameplayController(CameraSystem cameraSystem, Aimer aimer, IController controller)
        {
            _cameraSystem = cameraSystem;
            _aimer = aimer;

            PrivateSet(controller);
        }

        public void SetController(IController controller)
        {
            PrivateSet(controller);
        }

        public void HandleInput(InputData input)
        {
            var rotation = _aimer.Aim(input.Rotation);
            var position = _cameraSystem.MainCamera.GetFacedPosition(QueryTriggerInteraction.Ignore);

            _controller.HandleInput(input.Movement, position);

            _cameraSystem.ChangeCameraViewType(input.ViewModeIndex);
        }

        private void PrivateSet(IController controller)
        {
            _controller = controller;
            var preset = _controller.GetCameraTransformPreset();
            _cameraSystem.ApplyMainCameraPreset(preset);
        }

        private void CheckInteractions()
        {
            if (_cameraSystem.TryGetMainCameraFacedCollider(out var collider, LayerIds.BitMaskPlayer | LayerIds.BitMaskGround))
            {
                var gameObject = collider.gameObject;
                var layer = gameObject.layer;

                if (layer == LayerIds.IndexVendor
                    || layer == LayerIds.IndexGun)
                {
                    _view.ShowInteractionPrompt();
                }
            }
            else
            {
                _view.HideInteractionPrompt();
            }
        }
    }
}
