using Assets.Scripts.Player.Cannon.Projectile;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Spawn
{
    public class Spawner : MonoBehaviour
    {
        [Inject] private ObjectPool<Ball> _ballObjectPool;

        public Ball Spawn(Ball prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (_ballObjectPool.TryGet(out var ball))
            {
                ball.transform.SetLocalPositionAndRotation(position, rotation);
            }
            else
            {
                ball = Instantiate(prefab, position, rotation, parent);
                _ballObjectPool.Register(ball);
            }

            return ball;
        }
    }
}
