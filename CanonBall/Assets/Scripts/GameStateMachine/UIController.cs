using System.Collections.Generic;

namespace Assets.Scripts.GameStateMachine
{
    public class UIController
    {
        private readonly Dictionary<UIWindowTypes, IUIWindow> _windows = new();

        private IUIWindow _currentUIWindow;

        public UIWindowTypes? OpenWindow { get; private set; }

        public UIController(params IUIWindow[] windows)
        {
            foreach (var window in windows)
            {
                _windows[window.Type] = window;
            }
        }

        public void Open(UIWindowTypes type)
        {
            var window = _windows[type];
            Open(window);
        }

        public void Open(IUIWindow window)
        {
            ClearOpenWindow();

            _currentUIWindow = window;
            _currentUIWindow.Open();
            OpenWindow = window.Type;
        }


        public void ClearOpenWindow()
        {
            if (!OpenWindow.HasValue)
                return;

            _currentUIWindow.Close();
            _currentUIWindow = null;
            OpenWindow = null;
        }
    }

    public interface IUIWindow
    {
        public UIWindowTypes Type { get; }

        public void Open();
        public void Close();
    }
}
