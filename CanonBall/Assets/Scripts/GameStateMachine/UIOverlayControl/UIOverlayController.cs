using System.Collections.Generic;

namespace Assets.Scripts.GameStateMachine.UIOverlayControl
{
    public class UIOverlayController
    {
        private readonly Dictionary<OverlayType, IUIOverlay> _overlays = new();

        public UIOverlayController(params IUIOverlay[] overlays)
        {

            foreach (var overlay in overlays)
            {
                _overlays[overlay.Type] = overlay;
            }
        }

        public void Show(OverlayType type)
        {
            var overlay = _overlays[type];
            overlay.Show();
        }

        public void Hide(OverlayType type)
        {
            var overlay = _overlays[type];
            overlay.Hide();
        }
    }

    public interface IUIOverlay
    {
        public OverlayType Type { get; }

        public void Show();
        public void Hide();
    }
}
