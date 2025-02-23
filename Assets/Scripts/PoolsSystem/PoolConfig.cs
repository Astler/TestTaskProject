using System;
using System.Collections.Generic;
using UnityEngine;

namespace PoolsSystem
{
    [CreateAssetMenu(fileName = "PoolConfig", menuName = "Test project/Pool Config")]
    public class PoolConfig : ScriptableObject
    {
        [Serializable]
        public class PoolSettings
        {
            public GameObject prefab;
            public int defaultCapacity = 10;
        }

        public List<PoolSettings> settings = new();
    }
}