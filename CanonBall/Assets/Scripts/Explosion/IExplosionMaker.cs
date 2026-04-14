using System;
using UnityEngine;

namespace Assets.Scripts.Explosion
{
    public interface IExplosionMaker
    {
        public event Action<float, Vector3, float> ExplosionPerformed;
    }
}
