using UnityEngine;

namespace PoolsSystem
{
    public abstract class BasePoolable<T, TData> : MonoBehaviour, IPoolable<T, TData> where T : BasePoolable<T, TData>
    {
        protected PoolsSystem PoolsSystem { get; private set; }
        public TData Data { get; private set; }

        public void Initialize(PoolsSystem poolsSystem)
        {
            PoolsSystem = poolsSystem;
        }

        public virtual void OnCreate()
        {
        }

        public virtual void OnSpawned(TData spawnInfo)
        {
            Data = spawnInfo;
        }

        public virtual void OnDespawned()
        {
            Data = default;
        }

        public void Despawn()
        {
            PoolsSystem.Return(this as T);
        }
    }
}