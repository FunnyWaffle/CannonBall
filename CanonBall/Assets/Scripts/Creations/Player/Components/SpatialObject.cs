using Assets.Scripts.Space;
using System;
using UnityEngine;

namespace Assets.Scripts.Creations.Player.Components
{
    public class SpatialObject : ISpatialObject
    {
        public SpatialObject(Vector3 position)
        {
            Position = position;
        }

        public Vector3 Position { get; private set; }

        public event EventHandler<Vector3> PositionChanged;

        public void ChangePosition(Vector3 newPosition)
        {
            if (newPosition != Position)
            {
                Position = newPosition;
                PositionChanged?.Invoke(this, Position);
            }
        }
    }
}
