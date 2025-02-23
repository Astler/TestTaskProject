using UnityEngine;

namespace GamePhysics
{
    public static class TrajectoryPhysics
    {
        private static readonly RaycastHit[] HitBuffer = new RaycastHit[8];
        private const float MinPointDistance = 0.5f;

        private static Vector3 CalculatePositionAtTime(Vector3 startPosition, Vector3 initialVelocity, float time) =>
            startPosition + initialVelocity * time + ProjectConstants.Gravity * (0.5f * time * time);

        public static bool CalculateTrajectoryPoints(Vector3 startPosition, Vector3 initialVelocity, int pointsCount,
            float totalTime, LayerMask collisionMask, out Vector3[] points, out int actualPointCount)
        {
            points = new Vector3[pointsCount];
            actualPointCount = pointsCount;

            float timeStep = totalTime / (pointsCount - 1);
            Vector3 previousPoint = startPosition;
            points[0] = startPosition;
            int currentIndex = 1;

            for (int i = 1; i < pointsCount; i++)
            {
                float time = timeStep * i;
                Vector3 currentPoint = CalculatePositionAtTime(startPosition, initialVelocity, time);

                float distanceToLast = Vector3.Distance(currentPoint, previousPoint);
                if (distanceToLast < MinPointDistance) continue;

                Vector3 direction = currentPoint - previousPoint;
                float distance = direction.magnitude;

                int hitCount = Physics.RaycastNonAlloc(previousPoint, direction.normalized, HitBuffer, distance,
                    collisionMask);

                if (hitCount > 0)
                {
                    points[currentIndex] = HitBuffer[0].point;
                    actualPointCount = currentIndex + 1;
                    return true;
                }

                points[currentIndex] = currentPoint;
                previousPoint = currentPoint;
                currentIndex++;
            }

            actualPointCount = currentIndex;
            return false;
        }
    }
}