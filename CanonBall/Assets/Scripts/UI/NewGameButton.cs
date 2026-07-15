using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class NewGameButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public void Initialize()
        {
            _button.onClick.AddListener(OnNewGameButtonPressed);
        }

        private void OnNewGameButtonPressed()
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
