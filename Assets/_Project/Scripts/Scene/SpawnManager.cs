using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Flow
{
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Instance { get; private set; }

        private readonly Dictionary<string, SpawnInfo> spawns = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public GameObject Spawn(
            string spawnId,
            GameObject prefab,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null)
        {
            if (prefab == null)
            {
                Debug.LogWarning("SpawnManager : Prefab is null.");
                return null;
            }

            Register(
                spawnId,
                prefab,
                position,
                rotation,
                parent);

            SpawnInfo info = spawns[spawnId];

            if (info.Instance != null)
                return info.Instance;

            info.Instance = Instantiate(
                info.Prefab,
                info.Position,
                info.Rotation,
                info.Parent);

            FlowComponent[] flows =
                 info.Instance.GetComponentsInChildren<FlowComponent>(true);

            foreach (FlowComponent flow in flows)
            {
                FlowRegistry.Instance.Register(flow);
            }

            return info.Instance;
        }

        public void Destroy(string spawnId)
        {
            if (!spawns.TryGetValue(spawnId, out SpawnInfo info))
            {
                Debug.LogWarning(
                    $"SpawnManager : Spawn '{spawnId}' tidak ditemukan.");

                return;
            }

            if (info.Instance != null)
            {
                UnityEngine.Object.Destroy(info.Instance);
                info.Instance = null;
            }
        }

        public void DestroyById(string flowId)
        {
            FlowComponent component =
                FlowRegistry.Instance.Get<FlowComponent>(flowId);

            if (component == null)
            {
                Debug.LogWarning(
                    $"SpawnManager : Flow Id '{flowId}' tidak ditemukan.");

                return;
            }

            FlowRegistry.Instance.Unregister(flowId);
            UnityEngine.Object.Destroy(component.gameObject);
        }

        public bool IsSpawned(string spawnId)
        {
            return spawns.TryGetValue(spawnId, out SpawnInfo info)
                && info.Instance != null;
        }

        public GameObject GetInstance(string spawnId)
        {
            if (spawns.TryGetValue(spawnId, out SpawnInfo info))
                return info.Instance;

            return null;
        }

        public void Restore()
        {
            foreach (SpawnInfo info in spawns.Values)
            {
                if (info.Instance != null)
                    continue;

                info.Instance = Instantiate(
                    info.Prefab,
                    info.Position,
                    info.Rotation,
                    info.Parent);
            }
        }

        public void Register(
            string spawnId,
            GameObject prefab,
            Vector3 position,
            Quaternion rotation,
            Transform parent)
        {
            if (spawns.ContainsKey(spawnId))
                return;

            spawns.Add(spawnId, new SpawnInfo
            {
                SpawnId = spawnId,
                Prefab = prefab,
                Position = position,
                Rotation = rotation,
                Parent = parent
            });
        }

        public void RegisterGraph(FlowGraph graph)
        {
            foreach (FlowNode node in graph.Nodes)
            {
                if (node is SpawnNode spawnNode)
                {
                    spawnNode.Register();
                }
            }
        }
    }
}