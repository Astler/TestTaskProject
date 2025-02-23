using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class ValueByKey<K, V>
    {
        [field: SerializeField] public V Value { get; set; }
        [field: SerializeField] public K Key { get; set; }
    }
}