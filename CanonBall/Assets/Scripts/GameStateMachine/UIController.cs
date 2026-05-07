using Assets.Scripts.Input;
using System.Collections.Generic;

namespace Assets.Scripts.GameStateMachine
{
    public class UIController
    {
        private readonly Dictionary<UIWindowTypes, IUIWindow> _windows = new();

        private IUIWindow _currentUIWindow;

        public bool HasOpenWindow { get; private set; }

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
            HasOpenWindow = true;
        }

        private void ClearOpenWindow()
        {
            if (!HasOpenWindow)
                return;

            _currentUIWindow.Close();
            _currentUIWindow = null;
            HasOpenWindow = false;
        }

        public void HandleInput(InputData input)
        {
            if (input.IsBackEventPerformed)
                ClearOpenWindow();

            if (input.IsInventoryEventPerformed)
                if (_windows.TryGetValue(UIWindowTypes.Inventory, out var inventory))
                    if (!HasOpenWindow && _currentUIWindow != inventory)
                        Open(inventory);
                    else
                        ClearOpenWindow();
        }
    }

    public interface IUIWindow
    {
        public UIWindowTypes Type { get; }

        public void Open();
        public void Close();
    }
}
