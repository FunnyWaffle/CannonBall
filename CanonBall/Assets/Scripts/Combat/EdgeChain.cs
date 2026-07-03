using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class EdgeChain
    {
        private readonly List<AttackZoneEdge> _edges = new();

        public EdgeChain(Transform[] attackCorners)
        {
            _edges = new List<AttackZoneEdge>();
            InitializeEdges(attackCorners);
        }

        public IReadOnlyList<AttackZoneEdge> Edges => _edges;

        private void InitializeEdges(Transform[] attackCorners)
        {
            int length = attackCorners.Length;
            for (int i = 0; i < length; i++)
            {
                var startCorner = attackCorners[i];
                Transform endCorner;

                if (i == length - 1)
                {
                    endCorner = attackCorners[0];

                    if (Vector3.Normalize(startCorner.position - endCorner.position) ==
                        Vector3.Normalize(attackCorners[1].position - endCorner.position))
                        return;

                }
                else
                    endCorner = attackCorners[i + 1];

                if (Physics.Linecast(startCorner.position, endCorner.position, out var hit))
                    if (!hit.collider.isTrigger)
                        continue;

                _edges.Add(new AttackZoneEdge(startCorner, endCorner));
            }
        }
    }
}
