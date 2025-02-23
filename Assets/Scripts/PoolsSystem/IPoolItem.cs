namespace PoolsSystem
{
    public interface IPoolItem
    {
        void Initialize(PoolsSystem poolsSystem);
        void OnCreate();
        void OnDespawned();
    }
}