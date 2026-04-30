using Assets.Scripts.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.GameStateMachine
{
    public class GameStateMachine
    {
        private readonly List<IGameState> _states;

        private IGameState _currentState;

        public GameStateMachine(List<IGameState> states)
        {
            _states = new(states);

            _currentState = states.First();

            foreach (var state in _states)
            {
                state.StateEntered += SwitchState;
            }
        }

        public void HandleInput(InputData input)
        {
            _currentState.HandleInput(input);
        }

        private void SwitchState(object newState, EventArgs args)
        {
            _currentState = (IGameState)newState;
        }
    }
}
