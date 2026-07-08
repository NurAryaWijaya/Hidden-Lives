using System.Collections.Generic;
using UnityEngine;

namespace Game.Flow
{
    public class FlowRegistry : MonoBehaviour
    {
        public static FlowRegistry Instance { get; private set; }

        private readonly Dictionary<string, FlowComponent> registry =
            new Dictionary<string, FlowComponent>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            BuildRegistry();
        }

        private void BuildRegistry()
        {
            registry.Clear();

            FlowComponent[] components =
                FindObjectsByType<FlowComponent>(
                    FindObjectsSortMode.None);

            foreach (FlowComponent component in components)
            {
                if (string.IsNullOrWhiteSpace(component.Id))
                {
                    Debug.LogWarning(
                        $"{component.name} belum memiliki Flow Id.");

                    continue;
                }

                if (registry.ContainsKey(component.Id))
                {
                    Debug.LogWarning(
                        $"Flow Id '{component.Id}' sudah digunakan.");

                    continue;
                }

                registry.Add(component.Id, component);
            }
        }

        public FlowComponent Get(string id)
        {
            registry.TryGetValue(id, out FlowComponent component);

            return component;
        }

        public bool TryGet(string id, out FlowComponent component)
        {
            return registry.TryGetValue(id, out component);
        }

        public T Get<T>(string id)
        {
            if (!registry.TryGetValue(id, out FlowComponent flowComponent))
                return default;

            foreach (var component in flowComponent.GetComponents<MonoBehaviour>())
            {
                if (component is T target)
                    return target;
            }

            return default;
        }
    }
}