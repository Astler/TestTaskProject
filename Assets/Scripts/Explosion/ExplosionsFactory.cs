using PoolsSystem;
using UnityEngine;

namespace Explosion
{
    public class ExplosionsFactory
    {
        private readonly IPoolsSystem _poolsSystem;

        public ExplosionsFactory(IPoolsSystem poolsSystem)
        {
            _poolsSystem = poolsSystem;
        }

        public void Create(Vector3 position)
        {
            _poolsSystem.Get<ExplosionView, ExplosionSpawnInfo>(new ExplosionSpawnInfo(position));
        }
    }
}