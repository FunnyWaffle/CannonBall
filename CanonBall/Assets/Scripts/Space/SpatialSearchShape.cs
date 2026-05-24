using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Space
{
    public class SpatialSearchShape
    {
        private readonly Dictionary<int, List<Vector3Int>> _searchingRings = new();

        public IEnumerable<Vector3Int> GetShape(int radiusInCells)
        {
            return _searchingRings[radiusInCells];
        }

        public void BuldSearchingShape(int radiusInCells)
        {
            for (int r = 0; r < radiusInCells; r++)
            {
                if (_searchingRings.ContainsKey(r))
                    return;

                var offsets = new List<Vector3Int>();
                _searchingRings[r] = offsets;

                for (int x = -r; x <= r; x++)
                    for (int y = -r; y <= r; y++)
                        for (int z = -r; z <= r; z++)
                        {
                            if (Mathf.Max(
                                Mathf.Abs(x),
                                Mathf.Abs(y),
                                Mathf.Abs(z)) != r)
                            {
                                continue;
                            }

                            offsets.Add(new Vector3Int(x, y, z));
                        }
            }
        }
    }
}
