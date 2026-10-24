using UnityEngine;

namespace Assets.Scripts.Navigation.Surface
{
    [RequireComponent(typeof(MeshFilter))]
    public class NavMesh : MonoBehaviour
    {
#if UNITY_EDITOR

        [SerializeField] private MeshFilter _meshFilter;

        private void OnValidate()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }

        [ContextMenu("Create")]
        private void Create()
        {
            var mesh = _meshFilter.sharedMesh;

            var bounds = mesh.bounds;

            var vertices = mesh.vertices;
            var triangles = mesh.triangles;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                int aIndex = triangles[i],
                    bIndex = triangles[i + 1],
                    cIndex = triangles[i + 2];

                var a = vertices[aIndex];
                var b = vertices[bIndex];
                var c = vertices[cIndex];

                var min = Vector3.Min(Vector3.Min(a, b), c);
                var max = Vector3.Max(Vector3.Max(a, b), c);

                var center = (min + max) / 2;
                var halfExtents = center - min;
                var axis = Vector3.Normalize(Vector3.Cross(b - a, c - a));

                if (Check(center, halfExtents, axis))
                {

                }
            }
        }

        private bool Check(Vector3 center, Vector3 halfExtents, Vector3 direction)
        {
            if (!Physics.BoxCast(center, halfExtents, direction, out var hit))
                return false;

            return true;
        }

#endif
    }
}
