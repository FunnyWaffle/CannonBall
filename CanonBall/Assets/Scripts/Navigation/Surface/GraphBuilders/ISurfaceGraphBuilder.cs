using Assets.Scripts.Wrappers;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Navigation.Surface.GraphBuilders
{
    public interface ISurfaceGraphBuilder
    {
        public Task CreateAsyncNonAlloc(
            Vector3 cellStep,
            SerializableDictionary<Vector3Int, Vector3> graph);
    }
}
