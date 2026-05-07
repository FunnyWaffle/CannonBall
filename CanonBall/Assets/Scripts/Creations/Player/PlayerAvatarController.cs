using Assets.Scripts.Camera;
using Assets.Scripts.Config;
using Assets.Scripts.Creations.Player.Components;
using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Input;
using Assets.Scripts.Systems;
using R3;
using UnityEngine;

namespace Assets.Scripts.Creations.Player
{
    public class PlayerAvatarController : IUpdatable, IController
    {
        private readonly PlayerAvatarCore _core;
        private readonly PlayerAvatarView _view;

        private readonly CompositeDisposable _disposables = new();

        public PlayerAvatarController(PlayerAvatarView view,
            ConfigRepository configRepository,
            GameplayController gameplayState)
        {
            _view = view;

            _view.Initialize();
            _core = InitializeCore(configRepository);
            gameplayState.SetController(this);
        }

        public void Update()
        {
            CheckInteractions();
        }

        public void HandleInput(InputData input)
        {
            _core.Move(input.Movement);
            _core.Aim(input.Rotation);
            _core.SetViewMode(input.ViewModeIndex);

            if (input.IsInteractionPerformed)
                OnInteractionPerform();
        }

        public void TransferCamera()
        {
            CameraSystem.ApplyMainCameraPreset(_view.CameraPreset);
            _view.ShowCrosshair();
        }

        private void RotateView(Quaternion rotation)
        {
            _view.SetCameraPivotRotation(rotation);
        }

        private void CheckInteractions()
        {
            if (CameraSystem.TryGetMainCameraFacedCollider(out var collider, LayerIds.BitMaskPlayer | LayerIds.BitMaskGround))
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

        private void OnVelocityChange(Vector3 velocity)
        {
            var mainCamera = CameraSystem.MainCamera;
            var projectedVelocity = _core.Mover.ProjectVelocityOn(mainCamera.Forward, mainCamera.Right);
            _view.Move(projectedVelocity);
        }

        private void OnCameraViewTypeChange(CameraViewType cameraViewType)
        {
            _view.SetCameraViewType(cameraViewType);
            CameraSystem.ApplyMainCameraPreset(_view.CameraPreset);
        }

        private void OnInteractionPerform()
        {
            if (!CameraSystem.TryGetMainCameraFacedCollider(out _, LayerIds.BitMaskPlayer | LayerIds.BitMaskGround))
                return;

            _view.HideCrosshair();
        }

        private PlayerAvatarCore InitializeCore(ConfigRepository configRepository)
        {
            var config = configRepository.PlayerConfig;
            var aimer = CreateAimer(config);
            var mover = CreateMoverComtroller(config);

            var core = new PlayerAvatarCore(aimer, mover);

            _disposables.Add(core.CurrentViewType.Subscribe(OnCameraViewTypeChange));

            return core;
        }

        private Aimer CreateAimer(PlayerConfig config)
        {
            var aimer = new Aimer(config.Sensitivity, _view.CameraPreset.Pivot.eulerAngles);

            _disposables.Add(aimer.Rotation.Subscribe(RotateView));

            return aimer;
        }

        private PlayerAvatarMover CreateMoverComtroller(PlayerConfig config)
        {
            var mover = new PlayerAvatarMover(config.Speed);

            mover.VelocityChanged += OnVelocityChange;

            return mover;
        }
    }
}
