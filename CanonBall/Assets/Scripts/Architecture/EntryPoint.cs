using Assets.Scripts.Config;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class EntryPoint : MonoBehaviour
    {
        private async void Awake()
        {
            var configRepository = new ConfigRepository();
            await configRepository.LoadAsync();
            ProjectContext.Instance.Container.BindInterfacesAndSelfTo<ConfigRepository>().FromInstance(configRepository).AsSingle();

            SceneManager.LoadScene("SampleScene");
        }
    }
}
