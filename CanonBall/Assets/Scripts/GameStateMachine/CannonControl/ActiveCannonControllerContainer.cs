using Assets.Scripts.Systems;

namespace Assets.Scripts.GameStateMachine.CannonControl
{
    public class ActiveCannonControllerContainer
    {
        private readonly CameraSystem _cameraSystem;
        private readonly CrosshairSystem _crosshairSystem;

        private ICannonController _controller;

        public ActiveCannonControllerContainer(
            CameraSystem cameraSystem,
            CrosshairSystem crosshairSystem)
        {
            _cameraSystem = cameraSystem;
            _crosshairSystem = crosshairSystem;
        }

        public void SetController(ICannonController cannonController)
        {
            PrivateSet(cannonController);
        }

        public bool TryGetController(out ICannonController cannonController)
        {
            cannonController = _controller;
            return _controller != null;
        }

        public void ClearController()
        {
            _controller = null;
        }

        private void PrivateSet(ICannonController controller)
        {
            var preset = controller.GetCameraTransformPreset();
            _cameraSystem.ApplyMainCameraPreset(preset);
            _crosshairSystem.EnableCrosshair(CrosshairTypes.Cannon);
            _controller = controller;
        }
    }
}
