using System.Collections.Generic;
using UnityEngine;

namespace Game.Flow
{
    public class FlowGraphRegistry : MonoBehaviour
    {
        public static FlowGraphRegistry Instance { get; private set; }

        [SerializeField]
        private FlowGraph[] graphs;

        private readonly Dictionary<string, FlowGraph> graphLookup = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            BuildLookup();
        }

        private void BuildLookup()
        {
            graphLookup.Clear();

            foreach (FlowGraph graph in graphs)
            {
                if (graph == null)
                    continue;

                if (graphLookup.ContainsKey(graph.GraphId))
                {
                    Debug.LogWarning(
                        $"Graph Id '{graph.GraphId}' sudah digunakan.");
                    continue;
                }

                graphLookup.Add(graph.GraphId, graph);
            }
        }

        public bool TryGetGraph(
            string graphId,
            out FlowGraph graph)
        {
            return graphLookup.TryGetValue(
                graphId,
                out graph);
        }

        public FlowGraph GetGraph(string graphId)
        {
            graphLookup.TryGetValue(graphId, out FlowGraph graph);

            return graph;
        }
    }
}