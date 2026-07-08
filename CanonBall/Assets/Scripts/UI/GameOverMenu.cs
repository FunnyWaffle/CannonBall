using Assets.Scripts.GameStateMachine;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GameOverMenu : MonoBehaviour, IUIWindow
    {
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _exitButton;

        public UIWindowTypes Type => UIWindowTypes.GameOver;

        private void Start()
        {
            _newGameButton.onClick.AddListener(OnNewGameButtonClicked);
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        private void OnNewGameButtonClicked()
        {
            throw new NotImplementedException();
        }

        private void OnExitButtonClicked()
        {
            throw new NotImplementedException();
        }

        private void OnMainMenuButtonClicked()
        {
            throw new NotImplementedException();
        }
    }
}