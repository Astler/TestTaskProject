using GamePhysics;
using UnityEngine;

namespace Canon
{
    public class TrajectoryRenderer : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private int pointsCount = 25;
        [SerializeField] private float trajectoryTime = 2f;
        [SerializeField] private LayerMask collisionMask;

        private void Start()
        {
            InitializeLineRenderer();
        }

        private void InitializeLineRenderer()
        {
            lineRenderer.positionCount = pointsCount;
        }

        public void UpdateTrajectory(Vector3 startPoint, Vector3 initialVelocity)
        {
            TrajectoryPhysics.CalculateTrajectoryPoints(startPoint, initialVelocity, pointsCount, trajectoryTime,
                collisionMask, out Vector3[] points, out int actualPointCount);

            lineRenderer.positionCount = actualPointCount;
            lineRenderer.SetPositions(points);
        }
    }
}