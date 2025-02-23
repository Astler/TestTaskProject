using GameCamera;
using PoolsSystem;
using UnityEngine;

namespace Projectiles
{
    public class ProjectilesFactory
    {
        private readonly IPoolsSystem _poolsSystem;

        public ProjectilesFactory(IPoolsSystem poolsSystem)
        {
            _poolsSystem = poolsSystem;
        }

        public ProjectileView Create(Vector3 position, Vector3 direction, float power) =>
            _poolsSystem.Get<ProjectileView, ProjectileSpawnInfo>(new ProjectileSpawnInfo(position, direction, power));
    }
}