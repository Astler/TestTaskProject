using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PoolsSystem
{
    public class PoolsSystem : IPoolsSystem
    {
        private readonly Dictionary<Type, GameObject> _poolSettings = new();

        private readonly Dictionary<Type, Stack<GameObject>> _pools = new();
        private readonly PoolConfig _config;
        private readonly Transform _poolRoot;

        public PoolsSystem(PoolConfig config)
        {
            _config = config;
            _poolRoot = new GameObject("Pools Root").transform;
        }

        public void Initialize()
        {
            InitializePools(_config);
        }

        private void InitializePools(PoolConfig config)
        {
            foreach (PoolConfig.PoolSettings settings in config.settings)
            {
                _poolSettings[settings.prefab.GetComponent<IPoolItem>().GetType()] = settings.prefab;

                Type type = settings.prefab.GetComponent<IPoolItem>().GetType();
                Stack<GameObject> stack = new(settings.defaultCapacity);

                for (int i = 0; i < settings.defaultCapacity; i++)
                {
                    GameObject instance = CreateInstance(settings.prefab);
                    instance.SetActive(false);
                    stack.Push(instance);
                }

                _pools.Add(type, stack);
            }
        }

        private GameObject CreateInstance(GameObject prefab)
        {
            GameObject instance = UnityEngine.Object.Instantiate(prefab, _poolRoot);
            if (!instance.TryGetComponent(out IPoolItem poolable)) return instance;
            poolable.Initialize(this);
            poolable.OnCreate();

            return instance;
        }

        public T Get<T, TData>(TData data) where T : Component, IPoolable<T, TData>
        {
            if (!_pools.TryGetValue(typeof(T), out Stack<GameObject> stack))
                throw new Exception($"Pool for type {typeof(T)} not found");

            GameObject obj = stack.Count > 0 ? stack.Pop()
                : CreateInstance(_config.settings.First(s => s.prefab.GetComponent<T>()).prefab);
            obj.SetActive(true);
            T component = obj.GetComponent<T>();
            component.OnSpawned(data);
            return component;
        }

        public void Return<T>(T item) where T : Component
        {
            if (!_pools.TryGetValue(typeof(T), out Stack<GameObject> stack)) return;
            if (item.TryGetComponent(out IPoolItem poolable))
                poolable.OnDespawned();

            item.transform.SetParent(_poolRoot);
            item.gameObject.SetActive(false);
            stack.Push(item.gameObject);
        }
    }
}