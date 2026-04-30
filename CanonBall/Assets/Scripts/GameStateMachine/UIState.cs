using Assets.Scripts.Input;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameStateMachine
{
    public class UIState : IGameState
    {
        private readonly Dictionary<UIWindowTypes, IUIWindow> _windows;

        private IUIWindow _currentUIWindow;

        public event EventHandler StateEntered;

        public UIState(params IUIWindow[] windows)
        {
            foreach (var window in windows)
            {
                _windows[window.Type] = window;
                window.Initialize();
            }
        }

        public void Open(UIWindowTypes windowType)
        {
            _currentUIWindow.Close();
            _currentUIWindow = _windows[windowType];
            _currentUIWindow.Open();

            StateEntered?.Invoke(this, EventArgs.Empty);
        }

        public void Close(UIWindowTypes windowType)
        {
            var window = _windows[windowType];
            window.Close();
        }

        public void HandleInput(InputData input)
        {
            _currentUIWindow.HandleInput(input);
        }
    }

    public interface IUIWindow
    {
        public event Action Closed;

        public UIWindowTypes Type { get; }

        public void Initialize();
        public void Open();
        public void Close();
        public void HandleInput(InputData input);
    }
}
