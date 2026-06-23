using Assets.Scripts.Creations;
using Assets.Scripts.Creations.Placement;
using System;
using UnityEngine;

namespace Assets.Scripts.Build
{
    public class Construction : MonoBehaviour, IHasPosition, IComponent
    {
        [SerializeField] private Transform[] _parts;

        private int _currentPartIndex = 0;

        public bool Complete => _currentPartIndex == _parts.Length;
        public int PartCount => _parts.Length;

        public Vector3 Position => transform.position;

        public event Action PartAdded;

        public void BuildNextPart()
        {
            var part = _parts[_currentPartIndex];
            part.gameObject.SetActive(true);
            _currentPartIndex++;

            PartAdded?.Invoke();
        }
    }
}
