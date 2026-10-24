using Assets.Scripts.Wrappers;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Navigation.Surface.GraphBuilders
{
    public class TerrainBuilder : ISurfaceGraphBuilder
    {
        private readonly Terrain _terrain;

        public TerrainBuilder(Terrain terrain)
        {
            _terrain = terrain;
        }

        public async Task CreateAsyncNonAlloc(Vector3 cellStep,
            SerializableDictionary<Vector3Int, Vector3> graph)
        {
            var terrainData = _terrain.terrainData;

            var terrainSize = terrainData.size;

            var maxCell = terrainSize.FloorToInt(cellStep);

            for (int x = 0; x < maxCell.x; x++)
                for (int z = 0; z < maxCell.z; z++)
                {
                    var local = new Vector3(
                    x * cellStep.x,
                    0,
                    z * cellStep.z);

                    var worldPoint = _terrain.transform.TransformPoint(local);

                    worldPoint.y = _terrain.SampleHeight(worldPoint);

                    local = _terrain.transform.InverseTransformPoint(worldPoint);

                    graph.Add(local.FloorToInt(cellStep), local);
                }
        }
    }
}
