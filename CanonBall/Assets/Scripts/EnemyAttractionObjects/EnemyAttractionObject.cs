using Assets.Scripts.Combat;
using Assets.Scripts.Creations;
using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Input;
using Assets.Scripts.Space;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.EnemyAttractionObjects
{
    public class EnemyAttractionObject : MonoBehaviour, IDeathNotifier, IHasPosition, ISpatialObject
    {
        [SerializeField] private Transform[] _attackCorners;
        [SerializeField] private Collider[] _colliders;

        [SerializeField] private float _health;

        [Inject] private World _world;
        [Inject] private UIController _controller;
        [Inject] private InputSystem _inputSystem;

        public HitBox HitBox { get; private set; }
        public EntityComponents Components { get; private set; }
        public Vector3 Position => transform.position;

        public event Action Died;
        public event EventHandler<Vector3> PositionChanged;

        private void Start()
        {
            HitBox = new HitBox(_attackCorners, _colliders);

            var health = new Health(_health, _health);
            health.Died += OnDeath;

            Components = new EntityComponents();

            Components.Add(HitBox);
            Components.Add(health);
            Components.Add(this);

            foreach (var collider in _colliders)
            {
                _world.SpatialObjectsMap[collider] = this;
            }

            _world.EntityComponents[this] = Components;
        }

        private void OnDeath()
        {
            _controller.Open(UIWindowTypes.GameOver);
            _inputSystem.DisableCurrent();
        }
    }
}
