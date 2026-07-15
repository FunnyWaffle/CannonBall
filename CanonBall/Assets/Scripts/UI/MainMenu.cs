using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NewGameButton _newGameButton;
        [SerializeField] private QuitGameButton _quitGameButton;

        private void Start()
        {
            _newGameButton.Initialize();
            _quitGameButton.Initialize();
        }
    }
}
