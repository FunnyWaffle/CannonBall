using Assets.Scripts.Spawn;
using System.Collections.Generic;

namespace Assets.Scripts.Curency
{
    public class WaveCurrencyAccruer
    {
        private readonly WavesExecutor _wavesExecutor;
        private readonly List<ICurrencyReceiver<int>> _receivers;

        public WaveCurrencyAccruer(WavesExecutor wavesExecutor,
            List<ICurrencyReceiver<int>> receivers)
        {
            _wavesExecutor = wavesExecutor;
            _receivers = receivers;

            _wavesExecutor.WaveEnded += OnWaveEnded;
        }

        private void OnWaveEnded(int waveIndex)
        {
            const int coinPerWave = 5;

            foreach (var receiver in _receivers)
            {
                receiver.Add(waveIndex * coinPerWave);
            }
        }
    }
}
