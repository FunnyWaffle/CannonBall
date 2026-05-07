using Assets.Scripts.Input;

namespace Assets.Scripts.GameStateMachine
{
    public class GameplayController
    {
        private IController _controller;

        public void SetController(IController controller)
        {
            _controller = controller;
            _controller.TransferCamera();
        }

        public void HandleInput(InputData input)
        {
            _controller.HandleInput(input);
        }
    }
}
