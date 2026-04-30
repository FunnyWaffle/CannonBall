using UnityEngine;

namespace Assets.Scripts.Input
{
    public readonly struct InputData
    {
        public Vector2 Movement { get; }
        public Vector2 Rotation { get; }
        public int ViewModeIndex { get; }
        public bool IsInteractionPerformed { get; }
        public bool IsAttacked { get; }
        public bool IsBackEventPerformed { get; }
        public bool IsInventoryEventPerformed { get; }

        public InputData(
            Vector2 movement,
            Vector2 rotation,
            int viewModeIndex,
            bool isInteractionPerformed,
            bool isAttacked,
            bool isBackEventPerformed,
            bool isInventoryEventPerformed)
        {
            Movement = movement;
            Rotation = rotation;
            ViewModeIndex = viewModeIndex;
            IsInteractionPerformed = isInteractionPerformed;
            IsAttacked = isAttacked;
            IsBackEventPerformed = isBackEventPerformed;
            IsInventoryEventPerformed = isInventoryEventPerformed;
        }
    }
}
