using Assets.Scripts.GameStateMachine.UIControl;
using Assets.Scripts.GameStateMachine.UIWindowsControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GameOverMenu : MonoBehaviour, IUIWindow
    {
        [SerializeField] private NewGameButton _newGameButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private QuitGameButton _quitGameButton;

        public UIWindowTypes Type => UIWindowTypes.GameOver;

        private void Start()
        {
            _newGameButton.Initialize();
            _quitGameButton.Initialize();
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        private void OnMainMenuButtonClicked()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}