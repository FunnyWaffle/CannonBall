using Assets.Scripts.Combat;
using Assets.Scripts.Creations;
using Assets.Scripts.Creations.Placement;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Guns.Projections
{
    public class CannonProjectionView : MonoBehaviour, IPoolableObject, IPlaceable, IHasPosition, IHasRotation,
        IPositionable, IRotateable, IParentable, IEnableable, IDisabler, IConstructionProjection
    {
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _failureColor;

        [SerializeField] private Collider[] _colliders;
        [SerializeField] private Transform[] _attackCorners;

        [SerializeField] private Transform _buildCarriageSlot;

        [SerializeField] private CollisionNotifier[] _collisionNotifier;

        private List<MeshRenderer> _renderers = new();

        public Quaternion Rotation => transform.rotation;
        public Vector3 Position => transform.position;

        public Transform[] AttackCorners => _attackCorners;
        public Collider[] Colliders => _colliders;

        public float BuildCarriageOffset => Vector3.Distance(transform.position, _buildCarriageSlot.position);

        public event EventHandler<ItemTypes> Disabled;

        private void Start()
        {
            _renderers.AddRange(gameObject.GetComponentsInChildren<MeshRenderer>());
            SetRendererColor(_normalColor);

            foreach (var notifier in _collisionNotifier)
            {
                notifier.TriggerEntered += OnTriggerEnter;
                notifier.TriggerEntered += OnTriggerExit;
            }
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

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void Rotate(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void Place(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            SetPosition(position);
            Rotate(rotation);
            SetParent(parent);
        }
    }
}
