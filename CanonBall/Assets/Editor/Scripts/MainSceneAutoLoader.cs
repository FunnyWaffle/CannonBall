using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Assets.Editor.Scripts
{
    [InitializeOnLoad]
    public static class MainSceneAutoLoader
    {
        static MainSceneAutoLoader()
        {
            EditorApplication.delayCall += OnDelay;

            //EditorApplication.playModeStateChanged += OnPlayModeStateChange;
        }

        private static void OnDelay()
        {
            var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/EntryPoint.unity");
            EditorSceneManager.playModeStartScene = scene;
        }

        private static void OnPlayModeStateChange(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                SceneManager.LoadScene("EntryPoint");
            }
        }
    }
}
