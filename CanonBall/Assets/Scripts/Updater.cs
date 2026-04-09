using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Updater : MonoBehaviour
    {
        public event Action UpdatePerformed;

        private void Update()
        {
            UpdatePerformed?.Invoke();
        }
    }
}
