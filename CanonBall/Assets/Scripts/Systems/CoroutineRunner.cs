using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance = null;

        public static CoroutineRunner Instance => _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static Coroutine Start(IEnumerator enumerator)
        {
            return _instance.StartCoroutine(enumerator);
        }

        public static void Stop(Coroutine coroutine)
        {
            _instance.StopCoroutine(coroutine);
        }
    }
}
