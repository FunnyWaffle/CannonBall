using Assets.Scripts.Build;
using Assets.Scripts.Config;
using Assets.Scripts.Creations;
using Assets.Scripts.Creations.Placement;
using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Guns.Projections;
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
        private readonly List<IPlacementExecuter> _placementExecuters = new();

        private readonly Dictionary<ConstructionOrder, EntityComponents> _existingProjections = new();

        private readonly CameraSystem _cameraSystem;
        private readonly UIController _uIController;
        private readonly InputSystem _inputSystem;
        private readonly UniversalSpawner _spawner;
        private readonly BuildSystem _buildSystem;

        private EntityComponents _spawnedProjectionComponents;
        private ItemTypes _itemTipe;

        public PlaceObjectSystem(
            CameraSystem cameraSystem,
            UIController uIController,
            InputSystem inputSystem,
            UniversalSpawner spawner,
            BuildSystem buildSystem,
            params IPlacementExecuter[] placementExecuters)
        {
            _cameraSystem = cameraSystem;
            _uIController = uIController;
            _inputSystem = inputSystem;
            _spawner = spawner;
            _buildSystem = buildSystem;

            foreach (var executer in placementExecuters)
            {
                executer.PlacementStarted += ShowProjection;
                _placementExecuters.Add(executer);
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
            var constructionProjection = _spawnedProjectionComponents.Get<IConstructionProjection>();

            var order = await _buildSystem.OrderConstruction(hasPosition.Position, hasRotation.Rotation, constructionProjection.BuildCarriageOffset);

            order.ConstructionStarted += OnConstructionStart;

            _existingProjections[order] = _spawnedProjectionComponents;

            _spawnedProjectionComponents = null;
            IsPlacingObject = false;

            ObjectPlaced?.Invoke(_itemTipe);
        }

        public async void ShowProjection(ItemTypes itemType)
        {
            _itemTipe = itemType;
            var projectionType = _projections[itemType];

            _uIController.ClearOpenWindow();
            _inputSystem.SwitchToLast();

            var position = GetCameraFacedPosition();

            _spawnedProjectionComponents = await _spawner.SpawnAsync(projectionType, position, rotation: Quaternion.identity);

            IsPlacingObject = true;
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
            return _cameraSystem.MainCamera.GetFacedPosition(QueryTriggerInteraction.Ignore, LayerIds.BitMaskPlayer | LayerIds.BitMaskVendor);
        }

        private void OnConstructionStart(object sender, EventArgs e)
        {
            var order = (ConstructionOrder)sender;

            var projection = _existingProjections[order];

            var disabler = projection.Get<IDisabler>();
            disabler.Disable();
        }
    }
}
