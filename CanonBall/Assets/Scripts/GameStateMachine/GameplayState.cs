using Assets.Scripts.Input;
using System;

namespace Assets.Scripts.GameStateMachine
{
    public class GameplayState : IGameState
    {
        private IGameplayController _controller;

        public event EventHandler StateEntered;

        public void SetController(IGameplayController controller)
        {
            _controller = controller;
            _controller.TransferCamera();
            StateEntered?.Invoke(this, EventArgs.Empty);
        }

        public void HandleInput(InputData input)
        {
            _controller.HandleInput(input);
        }
    }
}
