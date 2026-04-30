using Assets.Scripts.Camera;
using Assets.Scripts.Config;
using Assets.Scripts.Creations.Player.Components;
using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Input;
using Assets.Scripts.Interaction;
using Assets.Scripts.Systems;
using R3;
using UnityEngine;

namespace Assets.Scripts.Creations.Player
{
    public class PlayerAvatarController : IUpdatable, IGameplayController
    {
        private readonly PlayerAvatarCore _core;
        private readonly PlayerAvatarView _view;

        private readonly GameplayState _gameplayState;
        private readonly UIState _uISatate;
        private readonly InteractionObjectsRepositiory _interactionObjectsRepositiory;
        private readonly Shop.Shop _shop;
        private readonly CompositeDisposable _disposables = new();

        public PlayerAvatarController(PlayerAvatarView view,
            InteractionObjectsRepositiory interactionObjectsRepositiory,
            ConfigRepository configRepository,
            Shop.Shop shop,
            GameplayState gameplayState,
            UIState uISatate)
        {
            _view = view;
            _interactionObjectsRepositiory = interactionObjectsRepositiory;
            _shop = shop;
            _gameplayState = gameplayState;
            _uISatate = uISatate;

            _view.Initialize();
            _core = InitializeCore(configRepository);
            gameplayState.SetController(this);

            _shop.Closed += _view.ShowCrosshair;
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
            if (!CameraSystem.TryGetMainCameraFacedCollider(out var collider, LayerIds.BitMaskPlayer | LayerIds.BitMaskGround))
                return;

            if (_interactionObjectsRepositiory.TryGetCannon(collider, out var cannonController))
            {
                _gameplayState.SetController(cannonController);
            }
            else if (collider.gameObject.layer == LayerIds.IndexVendor)
            {
                _uISatate.Open(UIWindowTypes.Shop);
            }

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

            aimer.RotationChanged += RotateView;

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
