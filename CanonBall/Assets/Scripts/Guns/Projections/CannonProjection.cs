using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Guns.Projections
{
    public class CannonProjection : MonoBehaviour, ISpawnable, IPoolableObject
    {
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _failureColor;

        private List<MeshRenderer> _renderers = new();

        public event EventHandler<ItemTypes> Disabled;

        private void Start()
        {
            _renderers.AddRange(gameObject.GetComponentsInChildren<MeshRenderer>());
            SetRendererColor(_normalColor);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            Disabled?.Invoke(this, ItemTypes.CannonProjection);
        }

        public void Place(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.SetParent(parent);
        }

        private void OnTriggerEnter(Collider other)
        {
            SetRendererColor(_failureColor);
        }

        private void OnTriggerExit(Collider other)
        {
            SetRendererColor(_normalColor);
        }

        private void SetRendererColor(Color color)
        {
            foreach (var renderer in _renderers)
            {
                renderer.material.color = color;
            }
        }
    }
}
