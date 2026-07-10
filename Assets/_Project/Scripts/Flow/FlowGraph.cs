using System.Collections.Generic;
using UnityEngine;

namespace Game.Flow
{
    [CreateAssetMenu(
        fileName = "Flow Graph",
        menuName = "Hidden Live/Flow/Flow Graph")]
    public class FlowGraph : ScriptableObject
    {
        [Header("Graph Info")]
        [SerializeField]
        private int chapterIndex;

        [SerializeField]
        private string graphId;

        [SerializeField]
        private string displayName;

        [TextArea]
        [SerializeField]
        private string description;

        [Header("Flow")]
        [SerializeField]
        private FlowNode startNode;

        [SerializeField]
        private List<FlowNode> nodes = new();
        public int ChapterIndex => chapterIndex;

        public string DisplayName => displayName;

        public string Description => description;

        public string GraphId => graphId;

        public FlowNode StartNode => startNode;

        public IReadOnlyList<FlowNode> Nodes => nodes;

        // Mengecek apakah node termasuk ke dalam graph ini.
        public bool Contains(FlowNode node)
        {
            return node != null && nodes.Contains(node);
        }

        // Mengambil node berdasarkan ID.
        public FlowNode GetNode(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId))
                return null;

            foreach (FlowNode node in nodes)
            {
                if (node.NodeId == nodeId)
                    return node;
            }

            return null;
        }

        public void AddNode(FlowNode node)
        {
            if (node == null)
                return;

            if (nodes.Contains(node))
                return;

#if UNITY_EDITOR
            node.Initialize();
#endif

            nodes.Add(node);
        }

        public void RemoveNode(FlowNode node)
        {
            if (node == null)
                return;

            nodes.Remove(node);

            if (startNode == node)
                startNode = null;
        }
    }
}