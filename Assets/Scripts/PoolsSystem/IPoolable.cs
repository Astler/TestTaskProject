using UnityEngine;

namespace PoolsSystem
{
    public interface IPoolable<T, TData> : IPoolItem where T : Component, IPoolable<T, TData>
    {
        TData Data { get; }
        void OnSpawned(TData spawnInfo);
    }
}