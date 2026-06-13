using Assets.Scripts.Creations;
using System;

namespace Assets.Scripts.Combat
{
    public interface IDeathNotifier : IComponent
    {
        public event Action Died;
    }
}
