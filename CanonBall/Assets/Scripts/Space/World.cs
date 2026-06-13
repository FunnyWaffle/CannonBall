using Assets.Scripts.Creations;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Space
{
    public class World
    {
        public Dictionary<Collider, ISpatialObject> SpatialObjectsMap { get; private set; } = new();
        public Dictionary<ISpatialObject, EntityComponents> EntityComponents { get; private set; } = new();
    }
}
