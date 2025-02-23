using UnityEngine;

namespace Projectiles
{
    public struct ProjectileSpawnInfo
    {
        public Vector3 StartWorldPosition { get; }
        public Vector3 Direction { get; }
        public float Power { get; }
        

        public ProjectileSpawnInfo(Vector3 startWorldPosition, Vector3 direction, float power)
        {
            StartWorldPosition = startWorldPosition;
            Direction = direction;
            Power = power;
        }
    }
}