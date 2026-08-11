using UnityEngine;

namespace Assets.Scripts.Wrappers
{
    public static class FloatExtensions
    {
        private static readonly float _epsilon = 0.00001f;

        public static int FloorToInt(
            this float value,
            float cellSize)
        {
            var nearestBoundary = Mathf.Round(value / cellSize) * cellSize;

            if (Mathf.Abs(value - nearestBoundary) <= _epsilon)
                value = nearestBoundary;

            return Mathf.FloorToInt(value / cellSize);
        }
    }
}
