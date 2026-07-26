using Assets.Scripts.Config;
using Assets.Scripts.Navigation;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Systems
{
    public class CustomPathFinder
    {
        private readonly NavMeshPath _navMeshPath = new();
        private readonly Collider[] _collidersFromRight = new Collider[10];
        private readonly Collider[] _collidersFromLeft = new Collider[10];

        private readonly float _extraOffsetFromCorner = 0.2f;

        private readonly UniversalSpawner _universalSpawner;

        public CustomPathFinder(UniversalSpawner universalSpawner)
        {
            _universalSpawner = universalSpawner;
        }

        public async Task<bool> TryFindAlloc(Vector3 sourcePosition, Vector3 sourceHalfExtents,
            Vector3 targetPosition, float rotationAngle, List<Vector3> path)
        {

            if (!NavMesh.CalculatePath(sourcePosition, targetPosition, ~NavMesh.GetAreaFromName("Not Passable For Carriage"), _navMeshPath))
                return false;

            var corners = _navMeshPath.corners;
            var cornersCount = corners.Length;

            var targetDirection = Vector3.Normalize(targetPosition - sourcePosition);
            var targetOrientation = Quaternion.LookRotation(targetDirection, Vector3.up);

            var previousCorner = corners[0];
            for (int i = 1; i < cornersCount; i++)
            {
                var corner = corners[i];
                var cornerCandidate = corner;

                var axis = corner - previousCorner;
                var direction = Vector3.Normalize(axis);
                var currentRotation = Quaternion.LookRotation(direction, Vector3.up);

                var forward = currentRotation * Vector3.forward;

                if (i == cornersCount - 1)
                {
                    var collidersOnLastCornerCount = Physics.OverlapBoxNonAlloc(corner, sourceHalfExtents, _collidersFromRight,
                        targetOrientation, ~LayerIds.BitMaskGround, QueryTriggerInteraction.Collide);

                    var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    cube.transform.position = corner;
                    cube.transform.rotation = targetOrientation;
                    cube.transform.localScale = sourceHalfExtents * 2;

                    var renderer = cube.GetComponent<MeshRenderer>();
                    renderer.material.color = Color.red;

                    if (collidersOnLastCornerCount >= 1)
                    {
                        var newCorner = corner + forward * (sourceHalfExtents.z + _extraOffsetFromCorner);

                        collidersOnLastCornerCount = Physics.OverlapBoxNonAlloc(corner, sourceHalfExtents, _collidersFromRight,
                        targetOrientation, ~LayerIds.BitMaskGround, QueryTriggerInteraction.Collide);

                        if (collidersOnLastCornerCount >= 1)
                            return false;

                        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                        sphere.transform.position = newCorner;

                        renderer = sphere.GetComponent<MeshRenderer>();
                        renderer.material.color = Color.red;

                        cornerCandidate = newCorner;
                    }
                }
                else
                {
                    var right = Vector3.Cross(forward, Vector3.up);

                    var xHalfExtents = sourceHalfExtents.x;
                    var xHalfExtentsWithOffset = xHalfExtents + _extraOffsetFromCorner;

                    var leftPositionCandidate = corner - right * xHalfExtentsWithOffset;
                    var rightPositionCandidate = corner + right * xHalfExtentsWithOffset;

                    var collidersFromLeftCount = Physics.OverlapBoxNonAlloc(leftPositionCandidate, sourceHalfExtents, _collidersFromLeft,
                        currentRotation, ~LayerIds.BitMaskGround, QueryTriggerInteraction.Collide);
                    var collidersFromRightCount = Physics.OverlapBoxNonAlloc(rightPositionCandidate, sourceHalfExtents, _collidersFromRight,
                        currentRotation, ~LayerIds.BitMaskGround, QueryTriggerInteraction.Collide);

                    if (collidersFromLeftCount == 0
                        && collidersFromRightCount == 0)
                        continue;

                    var hasObstacleFromRightSide = HasObstacleFromSide(_collidersFromRight, axis);
                    var hasObstacleFromLeftSide = HasObstacleFromSide(_collidersFromLeft, axis);

                    if (hasObstacleFromRightSide && hasObstacleFromLeftSide)
                    {
                        var notForVehicleZoneComponents = await _universalSpawner.SpawnAsync(ItemType.NotForVehicleZone, corner, currentRotation);

                        var notForVehicleZone = notForVehicleZoneComponents.Get<NotForVehicleZone>();
                        notForVehicleZone.AdjustSize(sourceHalfExtents * 2);

                        return await TryFindAlloc(previousCorner, sourceHalfExtents, targetPosition, rotationAngle, path);
                    }

                    if (hasObstacleFromRightSide && !hasObstacleFromLeftSide)
                        cornerCandidate = leftPositionCandidate;
                    else if (hasObstacleFromLeftSide && !hasObstacleFromRightSide)
                        cornerCandidate = rightPositionCandidate;
                    else if (!hasObstacleFromLeftSide && !hasObstacleFromRightSide)
                        cornerCandidate = rightPositionCandidate;

                    previousCorner = corner;
                }

                path.Add(cornerCandidate);
            }


            return true;
        }

        private bool HasObstacleFromSide(Collider[] colliders, Vector3 axis)
        {
            foreach (var collider in colliders)
            {
                if (collider == null)
                    break;

                var projection = Vector3.Dot(axis, collider.transform.position);

                if (projection >= -0.4 && projection <= 0.4)
                    return true;
            }

            return false;
        }
    }
}
