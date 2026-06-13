using Assets.Scripts.Creations;
using System.Collections.Generic;

namespace Assets.Scripts.Systems
{
    public class TimeSystem : IUpdatable
    {
        private readonly List<Timer> _timers = new();

        public Timer GetTimer(float duration)
        {
            return new Timer(duration);
        }

        public void Update()
        {
            foreach (var timer in _timers)
            {
                timer.Update();
            }
        }
    }

    public class Timer : ITimer
    {
        private readonly float _duration;

        public Timer(float duration)
        {
            _duration = duration;
        }

        public float Time { get; private set; }

        public bool Ready()
        {
            return Time >= _duration;
        }

        public void ShiftTime()
        {
            Time -= _duration;
        }

        public void Update()
        {
            Time += UnityEngine.Time.deltaTime;
        }
    }

    public interface ITimer
    {
        public float Time { get; }
    }
}
