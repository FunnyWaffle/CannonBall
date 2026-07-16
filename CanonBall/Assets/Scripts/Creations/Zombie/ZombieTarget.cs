using Assets.Scripts.Combat;
using Assets.Scripts.Creations.Placement;
using UnityEngine;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieTarget
    {
        private EntityComponents _entityComponents;
        private IPositionChangeNotifier _positionChangeNotifier;
        private IRotationChangeNotifier _rotationChangeNotifier;
        private IDeathNotifier _deathNotifier;
        private IHasPosition _hasPosition;
        private HitBox _hitBox;

        private Vector3 _lastPosition;
        private Vector3 _attackPositionOffset;

        private HitBoxAttackPlaceReservation _hitBoxAttackPlaceReservation;

        public Vector3 AttackPosition { get; private set; }
        public Vector3 Position => _hasPosition.Position;
        public bool IsSuitable => _hitBox != null;

        public void Set(
            EntityComponents components,
            HitBox hitBox,
            HitBoxAttackPlaceReservation hitBoxAttackPlaceReservation,
            Vector3 targetPosition)
        {
            if (components == null)
            {
                Clear();
                return;
            }

            _entityComponents = components;

            AttackPosition = targetPosition;

            ResetPositionChangeNotifier(components);
            ResetRotationChangeNotifier(components);
            ResetDeathNotifier(components);
            ResetPositionOwner(components);
            ResetRotationOwner(components);

            _hitBox?.ReleaseReservation(_hitBoxAttackPlaceReservation);
            _hitBox = hitBox;

            _hitBoxAttackPlaceReservation = hitBoxAttackPlaceReservation;
        }

        public bool Compare(EntityComponents components)
        {
            return _entityComponents == components;
        }

        private void ResetPositionChangeNotifier(EntityComponents components)
        {
            ClearPositionChangeNotifier();

            if (components != null
               && components.TryGet<IPositionChangeNotifier>(out var positionChangeNotifier))
            {
                _positionChangeNotifier = positionChangeNotifier;
                _positionChangeNotifier.PositionChanged += OnPositionChange;
            }
        }

        private void ResetRotationChangeNotifier(EntityComponents components)
        {
            ClearRotationChangeNotifier();

            if (components != null
               && components.TryGet<IRotationChangeNotifier>(out var rotationChangeNotifier))
            {
                _rotationChangeNotifier = rotationChangeNotifier;
                _rotationChangeNotifier.RotationChanged += OnRotationChange;
            }
        }

        private void ResetDeathNotifier(EntityComponents components)
        {
            ClearDeathNotifier();

            if (components.TryGet<IDeathNotifier>(out var deathNotifier))
            {
                _deathNotifier = deathNotifier;
                _deathNotifier.Died += OnDeath;
            }
        }

        private void ResetPositionOwner(EntityComponents components)
        {
            if (components.TryGet<IHasPosition>(out var hasPosition))
            {
                _hasPosition = hasPosition;
                _lastPosition = Position;
            }
        }

        private void ResetRotationOwner(EntityComponents components)
        {
            if (components.TryGet<IHasRotation>(out var hasRotation))
            {
                _attackPositionOffset = Quaternion.Inverse(hasRotation.Rotation) * (AttackPosition - Position);
            }
        }

        private void OnRotationChange(Quaternion rotation)
        {
            var newOffset = rotation * _attackPositionOffset;
            AttackPosition = Position + newOffset;
        }

        private void OnPositionChange(Vector3 position)
        {
            var delta = position - _lastPosition;
            AttackPosition += delta;

            _lastPosition = position;
        }

        private void OnDeath()
        {
            Clear();
        }

        private void Clear()
        {
            ClearPositionChangeNotifier();
            ClearRotationChangeNotifier();
            ClearDeathNotifier();

            _hasPosition = null;

            _hitBox?.ReleaseReservation(_hitBoxAttackPlaceReservation);
            _hitBox = null;
        }

        private void ClearPositionChangeNotifier()
        {
            if (_positionChangeNotifier == null)
                return;

            _positionChangeNotifier.PositionChanged -= OnPositionChange;
            _positionChangeNotifier = null;
        }

        private void ClearRotationChangeNotifier()
        {
            if (_rotationChangeNotifier == null)
                return;

            _rotationChangeNotifier.RotationChanged -= OnRotationChange;
            _rotationChangeNotifier = null;
        }

        private void ClearDeathNotifier()
        {
            if (_deathNotifier == null)
                return;

            _deathNotifier.Died -= OnDeath;
            _deathNotifier = null;
        }
    }
}
