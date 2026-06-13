using Assets.Scripts.Creations;
using System;
using UnityEngine;

namespace Assets.Scripts.Space
{
    public interface ISpatialObject : IHasPosition
    {

        public event EventHandler<Vector3> PositionChanged;
    }
}
