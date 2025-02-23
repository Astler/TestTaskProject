using UnityEngine;

namespace PoolsSystem
{
    public interface IPoolsSystem
    {
        T Get<T, TData>(TData data) where T : Component, IPoolable<T, TData>;

        void Return<T>(T item) where T : Component;
    }
}