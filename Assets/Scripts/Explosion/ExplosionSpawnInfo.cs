using UnityEngine;

namespace Explosion
{
    public struct ExplosionSpawnInfo
    {
        public Vector3 WorldPosition { get; }

        public ExplosionSpawnInfo(Vector3 worldPosition)
        {
            WorldPosition = worldPosition;
        }
    }
}