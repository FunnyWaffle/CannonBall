using Assets.Scripts.Config;
using Assets.Scripts.Creations;
using Assets.Scripts.Creations.Placement;
using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Input;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using Assets.Scripts.Systems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Placement
{
    public class PlaceObjectSystem : IUpdatable
    {
        private readonly Dictionary<ItemTypes, ItemTypes> _projections = new()
        {
            [ItemTypes.Cannon] = ItemTypes.CannonProjection
        };
        private readonly List<IPlacementExecutor> _placementExecuters = new();

        //private readonly Dictionary<ConstructionOrder, EntityComponents> _existingProjections = new();

        private readonly CameraSystem _cameraSystem;
        private readonly UIController _uIController;
        private readonly InputSystem _inputSystem;
        private readonly UniversalSpawner _spawner;
        //private readonly BuildSystem _buildSystem;

        private EntityComponents _spawnedProjectionComponents;
        private ItemTypes _itemTipe;

        public PlaceObjectSystem(
            CameraSystem cameraSystem,
            UIController uIController,
            InputSystem inputSystem,
            UniversalSpawner spawner,
            //BuildSystem buildSystem,
            params IPlacementExecutor[] placementExecutors)
        {
            _cameraSystem = cameraSystem;
            _uIController = uIController;
            _inputSystem = inputSystem;
            _spawner = spawner;
            //_buildSystem = buildSystem;

            foreach (var executor in placementExecutors)
            {
                executor.PlacementStarted += ShowProjection;
                _placementExecuters.Add(executor);
            }
        }

        public bool IsPlacingObject { get; private set; }

        public event Action<ItemTypes> ObjectPlaced;

        public async Task Place()
        {
            if (!IsPlacingObject)
                return;

            var hasPosition = _spawnedProjectionComponents.Get<IHasPosition>();
            var hasRotation = _spawnedProjectionComponents.Get<IHasRotation>();
            //var hasSize = _spawnedProjectionComponents.Get<IHasSize>();

            _ = _spawner.SpawnAsync(_itemTipe, hasPosition.Position, hasRotation.Rotation);

            //var order = await _buildSystem.OrderConstruction(hasPosition.Position, hasRotation.Rotation, hasSize.Size, Shape2D.Circle);

            //order.ConstructionStarted += OnConstructionStart;

            //_existingProjections[order] = _spawnedProjectionComponents;

            var disabler = _spawnedProjectionComponents.Get<IDisabler>();
            disabler.Disable();

            _spawnedProjectionComponents = null;
            IsPlacingObject = false;

            ObjectPlaced?.Invoke(_itemTipe);
        }

        public async void ShowProjection(ItemTypes itemType)
        {
            if (!IsPlacingObject)
            {
                _itemTipe = itemType;
                var projectionType = _projections[itemType];

                var position = GetCameraFacedPosition();

                _spawnedProjectionComponents = await _spawner.SpawnAsync(projectionType, position, rotation: Quaternion.identity);

                IsPlacingObject = true;
            }

            _uIController.ClearOpenWindow();
            _inputSystem.SwitchToLast();
        }

        public void Update()
        {
            MoveCurrentProjection();
        }

        private void MoveCurrentProjection()
        {
            if (!IsPlacingObject)
                return;

            var position = GetCameraFacedPosition();

            var placeable = _spawnedProjectionComponents.Get<IPlaceable>();
            placeable.Place(position, Quaternion.identity);
        }

        private Vector3 GetCameraFacedPosition()
        {
            var position = _cameraSystem.MainCamera.GetFacedPosition(QueryTriggerInteraction.Ignore, LayerIds.BitMaskPlayer | LayerIds.BitMaskVendor);
            return GetPositionUnder(position);
        }

        private Vector3 GetPositionUnder(Vector3 position)
        {
            if (Physics.Raycast(position, Vector3.down, out var hit, float.MaxValue, ~(LayerIds.BitMaskPlayer | LayerIds.BitMaskVendor), QueryTriggerInteraction.Ignore))
                return hit.point;
            else
                return position;
        }

        //private void OnConstructionStart(object sender, EventArgs e)
        //{
        //    var order = (ConstructionOrder)sender;

        //    var projection = _existingProjections[order];

        //    var disabler = projection.Get<IDisabler>();
        //    disabler.Disable();
        //}
    }
}
