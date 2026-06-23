using Assets.Scripts.Build.Carriage;
using Assets.Scripts.Creations;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Build
{
    public class ConstructionOrder
    {
        private readonly List<EntityComponents> _builders = new();

        private readonly UniversalSpawner _spawner;
        private readonly BuilderSpawnZone _builderSpawnZone;

        private readonly Vector3 _position;
        private readonly Quaternion _rotation;
        private readonly float _positionForCarriageOffset;

        private EntityComponents _carriageComponents;
        private EntityComponents _constructionComponents;

        public ConstructionOrder(
            Vector3 position,
            Quaternion rotation,
            float positionForCarriageOffset,
            BuilderSpawnZone builderSpawnZone,
            UniversalSpawner spawner)
        {
            _position = position;
            _rotation = rotation;
            _positionForCarriageOffset = positionForCarriageOffset;
            _builderSpawnZone = builderSpawnZone;
            _spawner = spawner;
        }

        public event EventHandler ConstructionStarted;

        public void SetCarriage(EntityComponents carriageComponents)
        {
            if (!carriageComponents.TryGet<IMover>(out var mover))
                return;

            _carriageComponents = carriageComponents;

            mover.Arrived += OnCarriagePathCompleteAsync;
        }

        public void SetBuilder(EntityComponents entityComponents)
        {
            if (!entityComponents.TryGet<Creations.Builder.Components.Build>(out _))
                return;

            _builders.Add(entityComponents);
        }

        private async void OnCarriagePathCompleteAsync()
        {
            _constructionComponents = await _spawner.SpawnAsync(ItemTypes.CannonToBuild, _position, _rotation);

            var construction = _constructionComponents.Get<Construction>();
            construction.PartAdded += OnConstructionPartAddition;

            ConstructionStarted?.Invoke(this, EventArgs.Empty);

            foreach (var builderComponents in _builders)
            {
                var build = builderComponents.Get<Creations.Builder.Components.Build>();

                var carriage = _carriageComponents.Get<IBuildCarriage>();

                build.Start(_constructionComponents, carriage);
            }
        }

        private void OnConstructionPartAddition()
        {
            var construction = _constructionComponents.Get<Construction>();

            if (construction.Complete)
            {
                var carriageMover = _carriageComponents.Get<IMover>();
                var position = _builderSpawnZone.GetPosition();
                carriageMover.SetDestination(position);
            }
        }
    }
}
