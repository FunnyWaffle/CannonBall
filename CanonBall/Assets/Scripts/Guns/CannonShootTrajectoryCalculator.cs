using Assets.Scripts.Config;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Guns
{
    public static class CannonShootTrajectoryCalculator
    {
        // Это я скопипастил у ChatGPT, так как не знаю, как это делать.
        // Комменты оставил, чтобы понимать, что тут вообще происходит.
        // Хотя тут ещё плюс минус понятно, просто надо знать формулы.
        // На удивление даже с первого раза дало +- рабочий код.
        public static Vector3 GetVelocity(Vector3 targetPosition, Vector3 startPosition, float power)
        {
            Vector3 direction = targetPosition - startPosition;
            Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);

            var angle = GetAngle(direction, directionXZ, power);
            return CulculateVelocity(directionXZ, power, angle);
        }

        public static List<Vector3> GetTrajectory(Vector3 startPosition, Vector3 velocity)
        {
            return BuildTrajectory(startPosition, velocity, Time.fixedDeltaTime);
        }

        private static float GetAngle(Vector3 direction, Vector3 directionXZ, float power)
        {
            float x = directionXZ.magnitude;
            float y = direction.y;

            float g = Mathf.Abs(Physics.gravity.y);

            float v2 = power * power;
            float v4 = v2 * v2;

            float root = v4 - g * (g * x * x + 2 * y * v2);

            if (root < 0)
            {
                return 0; // цель недостижима
            }

            float sqrt = Mathf.Sqrt(root);

            // два варианта угла — высокий и низкий
            float lowAngle = Mathf.Atan((v2 - sqrt) / (g * x));
            float highAngle = Mathf.Atan((v2 + sqrt) / (g * x));

            return lowAngle; // или highAngle, если нужен навес
        }

        private static Vector3 CulculateVelocity(Vector3 directionXZ, float power, float angle)
        {
            Vector3 velocity = Mathf.Cos(angle) * power * directionXZ.normalized;
            velocity.y = Mathf.Sin(angle) * power;
            return velocity;
        }

        private static List<Vector3> BuildTrajectory(Vector3 startPosition, Vector3 velocity, float deltaTime)
        {
            var points = new List<Vector3>();
            var steps = 50;

            Vector3 previousPosition = startPosition;

            for (int i = 0; i < steps; i++)
            {
                float t = i * deltaTime;
                Vector3 point = startPosition + velocity * t + 0.5f * Physics.gravity * t * t;

                if (Physics.Linecast(previousPosition, point, out var hit, ~LayerIds.Projectile))
                {
                    points.Add(hit.point);
                    break;
                }
                points.Add(point);
                previousPosition = point;
            }
            return points;
        }
    }
}
