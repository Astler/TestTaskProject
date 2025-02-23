using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class ComponentPool<T> where T : Component
    {
        private readonly List<T> _components = new();
        private readonly GameObject _parentObject;

        public ComponentPool(GameObject parent, int initialSize)
        {
            _parentObject = parent;
            
            for (int i = 0; i < initialSize; i++)
            {
                AddComponent();
            }
        }

        private T AddComponent()
        {
            T component = _parentObject.AddComponent<T>();
            _components.Add(component);
            return component;
        }

        public T GetAvailableComponent(System.Predicate<T> condition)
        {
            foreach (T component in _components)
            {
                if (condition(component))
                {
                    return component;
                }
            }

            return AddComponent();
        }
    }
}