using Assets.Scripts.Input;
using System;

namespace Assets.Scripts.GameStateMachine
{
    public interface IGameState
    {
        public event EventHandler StateEntered;
        public void HandleInput(InputData input);
    }
}
