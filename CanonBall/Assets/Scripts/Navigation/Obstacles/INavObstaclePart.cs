using UnityEngine;

namespace Assets.Scripts.Navigation.Obstacles
{
    public interface INavObstaclePart
    {
        public Bounds Bounds { get; }
    }
}
