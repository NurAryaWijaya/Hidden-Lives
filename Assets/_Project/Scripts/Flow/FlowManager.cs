using System;
using UnityEngine;

namespace Game.Flow
{
    public class FlowManager : MonoBehaviour
    {
        public static FlowManager Instance { get; private set; }

        public event Action<FlowGraph> FlowStarted;
        public event Action<FlowNode> FlowNodeChanged;
        public event Action FlowEnded;
        public event Action GraphFinished;

        public FlowGraph CurrentGraph { get; private set; }

        public FlowNode CurrentNode { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        // Memulai FlowGraph.
        public void StartFlow(FlowGraph graph)
        {
            if (graph == null)
            {
                Debug.LogError("Cannot start Flow. Graph is null.");
                return;
            }

            if (CurrentNode != null)
            {
                CurrentNode.Completed -= HandleNodeCompleted;
                CurrentNode.Exit();

                CurrentNode = null;
            }

            CurrentGraph = graph;

            FlowStarted?.Invoke(graph);

            GoTo(graph.StartNode);
        }
        public void StartFlow(FlowGraph graph,FlowNode startNode)
        {
            if (graph == null)
            {
                Debug.LogError("Cannot start Flow. Graph is null.");
                return;
            }

            if (CurrentNode != null)
            {
                CurrentNode.Completed -= HandleNodeCompleted;
                CurrentNode.Exit();

                CurrentNode = null;
            }

            CurrentGraph = graph;

            FlowStarted?.Invoke(graph);

            GoTo(startNode);
        }

        // Berpindah ke FlowNode berikutnya.
        public void GoTo(FlowNode node)
        {
            if (node == null)
            {
                EndFlow();
                return;
            }

            if (!CurrentGraph.Contains(node))
            {
                Debug.LogError($"FlowNode '{node.name}' does not belong to FlowGraph '{CurrentGraph.name}'.");
                return;
            }

            if (CurrentNode != null)
            {
                CurrentNode.Completed -= HandleNodeCompleted;
                CurrentNode.Exit();
            }

            CurrentNode = node;

            CurrentNode.Completed += HandleNodeCompleted;

            FlowNodeChanged?.Invoke(CurrentNode);

            CurrentNode.Enter();
        }

        // Kembali ke awal graph.
        public void Restart()
        {
            if (CurrentGraph == null)
                return;

            GoTo(CurrentGraph.StartNode);
        }

        // Lompat ke node tertentu berdasarkan ID.
        // Berguna untuk replay/checkpoint
        public void JumpTo(FlowNode node)
        {
            GoTo(node);
        }

        private void HandleNodeCompleted(FlowNode nextNode)
        {
            if (nextNode == null)
            {
                FinishGraph();
                return;
            }

            GoTo(nextNode);
        }

        private void EndFlow()
        {
            if (CurrentNode != null)
            {
                CurrentNode.Completed -= HandleNodeCompleted;
                CurrentNode.Exit();
            }

            CurrentNode = null;
            CurrentGraph = null;

            FlowEnded?.Invoke();
        }

        private void FinishGraph()
        {
            CurrentNode = null;

            GraphFinished?.Invoke();

            Debug.Log("Flow Graph Finished");
        }
    }
}