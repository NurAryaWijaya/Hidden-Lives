using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Flow
{
    public class CheckpointManager : MonoBehaviour
    {
        public static CheckpointManager Instance { get; private set; }

        public event Action<string> CheckpointUnlocked;

        private readonly Dictionary<string, CheckpointInfo> checkpoints =
            new();
        private CheckpointInfo pendingCheckpoint;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void Unlock(
            string checkpointId,
            string sceneName,
            FlowGraph graph,
            FlowNode node)
        {
            if (string.IsNullOrWhiteSpace(checkpointId))
                return;

            CheckpointInfo info = new()
            {
                CheckpointId = checkpointId,
                SceneName = sceneName,
                GraphId = graph.GraphId,
                NodeId = node.NodeId,

                Snapshot = WorldStateManager.Instance.CreateSnapshot()
            };

            checkpoints[checkpointId] = info;

            CheckpointUnlocked?.Invoke(checkpointId);

            Debug.Log($"Checkpoint '{checkpointId}' unlocked.");
        }

        public bool IsUnlocked(string checkpointId)
        {
            return checkpoints.ContainsKey(checkpointId);
        }

        public bool TryGetCheckpoint(
            string checkpointId,
            out CheckpointInfo checkpoint)
        {
            return checkpoints.TryGetValue(
                checkpointId,
                out checkpoint);
        }

        public IEnumerable<CheckpointInfo> GetAll()
        {
            return checkpoints.Values;
        }

        public void Clear()
        {
            checkpoints.Clear();
        }

        //Load system
        public void Load(string checkpointId)
        {
            if (!checkpoints.TryGetValue(checkpointId, out CheckpointInfo checkpoint))
            {
                Debug.LogWarning($"Checkpoint '{checkpointId}' tidak ditemukan.");
                return;
            }

            pendingCheckpoint = checkpoint;

            CheckpointCanvas.Instance.Hide();

            SceneLoader.Instance.LoadScene(checkpoint.SceneName);
        }

        private void OnEnable()
        {
            if (SceneLoader.Instance != null)
                SceneLoader.Instance.SceneLoaded += HandleSceneLoaded;
        }

        private void OnDisable()
        {
            if (SceneLoader.Instance != null)
                SceneLoader.Instance.SceneLoaded -= HandleSceneLoaded;
        }

        private void HandleSceneLoaded()
        {
            if (pendingCheckpoint == null)
                return;

            FlowGraph graph =
                FlowGraphRegistry.Instance.GetGraph(
                    pendingCheckpoint.GraphId);

            if (graph == null)
            {
                Debug.LogError(
                    $"FlowGraph '{pendingCheckpoint.GraphId}' tidak ditemukan.");

                pendingCheckpoint = null;
                return;
            }

            FlowNode node =
                graph.GetNode(
                    pendingCheckpoint.NodeId);

            if (node == null)
            {
                Debug.LogError(
                    $"Node '{pendingCheckpoint.NodeId}' tidak ditemukan.");

                pendingCheckpoint = null;
                return;
            }

            WorldStateManager.Instance.RestoreSnapshot(pendingCheckpoint.Snapshot);

            WorldStateManager.Instance.Apply();

            FlowManager.Instance.StartFlow(graph, node);

            pendingCheckpoint = null;
        }
    }
}