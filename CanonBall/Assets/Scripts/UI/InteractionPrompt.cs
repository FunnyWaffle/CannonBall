using Assets.Scripts.GameStateMachine.UIOverlayControl;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class InteractionPrompt : MonoBehaviour, IUIOverlay
    {
        public OverlayType Type => OverlayType.InteractionPrompt;

        private void Start()
        {
            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
