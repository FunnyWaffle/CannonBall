using Assets.Scripts.Navigation.Agent;
using Assets.Scripts.Navigation.Surface;
using Assets.Scripts.Systems;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Navigation.Path
{
    [Serializable]
    public class PathSmoother
    {
        [SerializeField] private float _pathSimplificationTolerance = .5f;

        private readonly List<Vector3> _smoothedPath = new();

        private void RemoveCollinearNodes(List<Vector3> path)
        {
            var node = path[0];
            var nextNode = path[1];

            var previousDirection = Vector3.Normalize(nextNode - node);

            _smoothedPath.Add(node);

            for (int i = 1; i < path.Count - 1; i++)
            {
                node = path[i];
                nextNode = path[i + 1];

                var direction = Vector3.Normalize(nextNode - node);

                if (direction != previousDirection)
                {
                    previousDirection = direction;
                    _smoothedPath.Add(node);
                }
            }

            _smoothedPath.Add(path[^1]);
        }

        private bool CanMoveDirectly(Collider[] agentColliders, Vector3 from, Vector3 to)
        {
            return !CollisionCalculator.Intersect(agentColliders, from, to);
        }

        public void StringPulling(
            List<Vector3> path,
            float stepLength,
            float allowedDropHeight,
            NavAgent agent,
            SurfaceSequence surfaceSequence)
        {
            _smoothedPath.Clear();

            _smoothedPath.Add(path[0]);

            int anchor = 0;

            while (anchor < path.Count - 1)
            {
                int furthest = anchor + 1;

                for (int candidate = path.Count - 1;
                     candidate > anchor;
                     candidate--)
                {
                    if (CanGoStraight(
                        path[anchor],
                        path[candidate],
                        stepLength,
                        agent.StepHeight,
                        allowedDropHeight,
                        surfaceSequence))
                    {
                        furthest = candidate;
                        break;
                    }
                }

                _smoothedPath.Add(path[furthest]);
                anchor = furthest;
            }

            path.Clear();

            path.AddRange(_smoothedPath);
        }

        public bool CanGoStraight(
            Vector3 start,
            Vector3 end,
            float stepLength,
            float stepHeight,
            float allowedDropHeight,
            SurfaceSequence surfaceSequence)
        {
            var axis = end - start;

            var previousHeight = end.y;

            var direction = Vector3.Normalize(axis);

            for (float j = axis.magnitude - stepLength; j > 0; j -= stepLength)
            {
                var point = start + direction * j;

                var height = surfaceSequence.SampleHeight(point);

                var heightDifference = height - previousHeight;

                if (heightDifference > stepHeight
                    || heightDifference < -allowedDropHeight)
                {
                    return false;
                }

                previousHeight = height;
            }

            return true;
        }
    }
}
