using UnityEngine;

namespace Assets.Scripts.Wrappers
{
    public static class Vector3Extensions
    {
        public static Vector2 ToXZ(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        public static Vector3Int FloorToInt(
            this Vector3 vector,
            float cellSize)
        {
            var flatX = vector.x.FloorToInt(cellSize);
            var flatY = vector.y.FloorToInt(cellSize);
            var flatZ = vector.z.FloorToInt(cellSize);

            return new Vector3Int(flatX, flatY, flatZ);
        }

        public static Vector2Int FloorToIntXZ(this Vector3 vector, float cellSize)
        {
            var flatX = vector.x.FloorToInt(cellSize);
            var flatZ = vector.z.FloorToInt(cellSize);

            return new Vector2Int(flatX, flatZ);
        }
    }
}
