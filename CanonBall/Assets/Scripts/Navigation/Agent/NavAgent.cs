using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Navigation.Agent
{
    public class NavAgent : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _angularSpeed;

        [SerializeField] private Collider[] _movementCollision;

        public Vector3 Position => transform.position;
        public Collider[] MovementCollision => _movementCollision;

        public event Action GotNewPath;
        public event Action PathCompleted;

        public IEnumerator Set(IReadOnlyList<Vector3> path)
        {
            var enumerator = FollowPath(path);

            GotNewPath?.Invoke();

            return enumerator;
        }

        private IEnumerator FollowPath(IReadOnlyList<Vector3> path)
        {
            foreach (var node in path)
            {
                yield return MoveTo(node);
                yield return RotateTo(node);
            }

            PathCompleted?.Invoke();
        }

        private IEnumerator MoveTo(Vector3 target)
        {
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(
                            transform.position,
                            target,
                            _speed * Time.deltaTime);

                yield return null;
            }
        }

        private IEnumerator RotateTo(Vector3 position)
        {
            var direction = Vector3.Normalize(position - transform.position);
            var rotation = Quaternion.LookRotation(direction, Vector3.up);

            while (Quaternion.Angle(transform.rotation, rotation) > 1f)
            {
                transform.rotation = Quaternion.RotateTowards(
                            transform.rotation,
                            rotation,
                            _angularSpeed * Time.deltaTime);

                yield return null;
            }
        }
    }
}
