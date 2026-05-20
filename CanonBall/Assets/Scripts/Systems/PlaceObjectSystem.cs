using Assets.Scripts.Config;
using Assets.Scripts.Creations;
using Assets.Scripts.Guns;
using Assets.Scripts.Guns.Projections;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class PlaceObjectSystem : IUpdatable, ISpawnRequester<CannonProjection>, ISpawnRequester<CannonController>
    {
        private readonly Dictionary<ItemTypes, ItemTypes> _projections = new()
        {
            [ItemTypes.Cannon] = ItemTypes.CannonProjection
        };
        private readonly CameraSystem _cameraSystem;

        private CannonProjection _spawnedProjection;
        private ItemTypes _itemType;

        public PlaceObjectSystem(CameraSystem cameraSystem)
        {
            _cameraSystem = cameraSystem;
        }

        public bool IsPlacingObject => _spawnedProjection != null;

        private EventHandler<SpawnArguments> _cannonProjectionSpawnRequest;
        private EventHandler<SpawnArguments> _cannonControllerSpawnRequest;

        event EventHandler<SpawnArguments> ISpawnRequester<CannonProjection>.SpawnRequested
        {
            add => _cannonProjectionSpawnRequest += value;
            remove => _cannonProjectionSpawnRequest -= value;
        }

        event EventHandler<SpawnArguments> ISpawnRequester<CannonController>.SpawnRequested
        {
            add => _cannonControllerSpawnRequest += value;
            remove => _cannonControllerSpawnRequest -= value;
        }

        public event Action<ItemTypes> ObjectPlaced;

        public void Place()
        {
            if (!IsPlacingObject)
                return;

            var position = GetCameraFacedPosition();

            _cannonControllerSpawnRequest?.Invoke(this, new SpawnArguments(_itemType, position, rotation: Quaternion.identity));
        }

        public void ShowProjection(ItemTypes itemType)
        {
            _itemType = itemType;
            var projection = _projections[itemType];

            var position = GetCameraFacedPosition();

            _cannonProjectionSpawnRequest?.Invoke(this, new SpawnArguments(projection, position, rotation: Quaternion.identity));
        }

        public void SetSpawnedObject(CannonProjection cannonProjection)
        {
            _spawnedProjection = cannonProjection;
        }

        public void SetSpawnedObject(CannonController obj)
        {
            _spawnedProjection.Disable();
            _spawnedProjection = null;

            ObjectPlaced?.Invoke(_itemType);
        }

        public void Update()
        {
            if (!IsPlacingObject)
                return;

            var position = GetCameraFacedPosition();

            _spawnedProjection.Place(position, Quaternion.identity);
        }

        private Vector3 GetCameraFacedPosition()
        {
            return _cameraSystem.MainCamera.GetFacedPosition(QueryTriggerInteraction.Ignore, LayerIds.BitMaskPlayer | LayerIds.BitMaskVendor);
        }
    }
}
