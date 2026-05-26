using System;
using UnityEngine;

namespace Assets.Scripts.Space
{
    public interface ISpatialObject
    {
        public Vector3 Position { get; }

        public event EventHandler<Vector3> PositionChanged;
    }
}
