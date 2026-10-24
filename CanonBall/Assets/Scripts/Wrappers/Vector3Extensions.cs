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

        public static Vector3Int FloorToInt(
            this Vector3 vector,
            Vector3 cellSizes)
        {
            var flatX = vector.x.FloorToInt(cellSizes.x);
            var flatY = vector.y.FloorToInt(cellSizes.y);
            var flatZ = vector.z.FloorToInt(cellSizes.z);

            return new Vector3Int(flatX, flatY, flatZ);
        }

        public static Vector2Int FloorToIntXZ(this Vector3 vector, float cellSize)
        {
            var flatX = vector.x.FloorToInt(cellSize);
            var flatZ = vector.z.FloorToInt(cellSize);

            return new Vector2Int(flatX, flatZ);
        }

        public static bool IsLess(this Vector3 first, Vector3 sectond)
        {
            return first.x <= sectond.x
                && first.y <= sectond.y
                && first.z <= sectond.z;
        }

        public static bool IsBigger(this Vector3 first, Vector3 sectond)
        {
            return first.x >= sectond.x
                && first.y >= sectond.y
                && first.z >= sectond.z;
        }
    }
}
