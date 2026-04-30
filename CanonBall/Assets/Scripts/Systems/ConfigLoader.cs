using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Systems
{
    public static class ConfigLoader
    {
        public static async Task<T> Load<T>(string name)
            where T : ScriptableObject
        {
            var task = Addressables.LoadAssetAsync<T>(name);
            await task.Task;
            return task.Result;
        }
    }
}
