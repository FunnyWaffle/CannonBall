using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class QuitGameButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public void Initialize()
        {
            _button.onClick.AddListener(OnGameQuit);

#if UNITY_EDITOR
            _button.onClick.AddListener(OnRuntimeModQuit);
#endif
        }

        private void OnGameQuit()
        {
            Application.Quit();
        }

#if UNITY_EDITOR

        private void OnRuntimeModQuit()
        {
            UnityEditor.EditorApplication.ExitPlaymode();
        }

#endif
    }
}
