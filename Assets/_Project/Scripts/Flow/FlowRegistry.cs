using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        }

        private void BuildRegistry()
        {
            registry.Clear();

            FlowComponent[] components = FindObjectsByType<FlowComponent>(FindObjectsInactive.Include, FindObjectsSortMode.None);

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
            Debug.Log($"FlowRegistry : {registry.Count} component(s) loaded.");
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

            // Object sudah dihancurkan
            if (flowComponent == null)
            {
                registry.Remove(id);
                return default;
            }

            return flowComponent.GetComponent<T>();
        }

        public void Unregister(string id)
        {
            registry.Remove(id);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            BuildRegistry();
        }

        //Gar bisa dipanggil siste lain
        public void Refresh()
        {
            BuildRegistry();
        }

        public void Register(FlowComponent component)
        {
            if (component == null)
                return;

            if (string.IsNullOrWhiteSpace(component.Id))
                return;

            if (registry.ContainsKey(component.Id))
            {
                Debug.LogWarning($"Flow Id '{component.Id}' sudah terdaftar.");
                return;
            }

            registry.Add(component.Id, component);
        }
    }
}