using UnityEngine;

namespace Assets.Scripts.Wrappers
{
    public static class Vector3Extensions
    {
        public static Vector2 ToXZ(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        public static Vector3Int FloorToInt(this Vector3 vector, float cellSize)
        {
            var scaledVector = vector / cellSize;

            var flatX = Mathf.FloorToInt(scaledVector.x);
            var flatY = Mathf.FloorToInt(scaledVector.y);
            var flatZ = Mathf.FloorToInt(scaledVector.z);

            return new Vector3Int(flatX, flatY, flatZ);
        }

        public static Vector2Int FloorToIntXZ(this Vector3 vector, float cellSize)
        {
            var scaledVector = vector / cellSize;

            var flatX = Mathf.FloorToInt(scaledVector.x);
            var flatZ = Mathf.FloorToInt(scaledVector.z);

            return new Vector2Int(flatX, flatZ);
        }
    }
}
