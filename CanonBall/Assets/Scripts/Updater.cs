using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Updater : MonoBehaviour
    {
        public event Action UpdatePerformed;
        public event Action LateUpdatePerformed;

        private void Update()
        {
            UpdatePerformed?.Invoke();
        }

        private void LateUpdate()
        {
            LateUpdatePerformed?.Invoke();
        }
    }
}
