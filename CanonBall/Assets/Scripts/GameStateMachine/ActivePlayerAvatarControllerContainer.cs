using Assets.Scripts.Systems;

namespace Assets.Scripts.GameStateMachine
{
    public class ActivePlayerAvatarControllerContainer
    {
        private readonly CameraSystem _cameraSystem;
        private readonly CrosshairSystem _crosshairSystem;

        private IPlayerAvatarController _controller;
        private IPlayerAvatarController _lastController;

        public ActivePlayerAvatarControllerContainer(
            CameraSystem cameraSystem,
            CrosshairSystem crosshairSystem)
        {
            _cameraSystem = cameraSystem;
            _crosshairSystem = crosshairSystem;
        }

        public void SetController(IPlayerAvatarController controller)
        {
            _controller?.Stop();
            _lastController = _controller;
            PrivateSet(controller);
        }

        public void ClearController()
        {
            _lastController = _controller;
            _controller.Stop();
            _controller = null;
        }

        public void SetLastController()
        {
            PrivateSet(_lastController);
            _lastController = null;
        }

        public bool TryGetController(out IPlayerAvatarController controller)
        {
            controller = _controller;
            return _controller != null;
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
