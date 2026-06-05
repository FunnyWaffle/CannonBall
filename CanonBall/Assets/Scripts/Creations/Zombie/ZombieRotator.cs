using Assets.Scripts.Systems;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieRotator
    {
        private readonly ZombieView _view;
        private readonly ZombieTarget _zombieTarget;

        private Coroutine _coroutine;

        public ZombieRotator(ZombieView view, ZombieTarget zombieTarget)
        {
            _view = view;
            _zombieTarget = zombieTarget;
        }

        public void RotateToTarget()
        {
            var direction = _zombieTarget.Target.Position - _view.Position;
            var rotation = Quaternion.LookRotation(direction, Vector3.up);

            if (_coroutine != null)
                CoroutineRunner.Instance.StopCoroutine(_coroutine);

            _coroutine = CoroutineRunner.Instance.StartCoroutine(Rotate(rotation));
        }

        private IEnumerator Rotate(Quaternion rotation)
        {
            Quaternion currentRotation = _view.Rotation;

            while (currentRotation != rotation)
            {
                currentRotation = Quaternion.RotateTowards(currentRotation, rotation, _view.RotationSpeed * Time.deltaTime);
                _view.Rotate(currentRotation);

                yield return null;
            }
        }
    }
}
