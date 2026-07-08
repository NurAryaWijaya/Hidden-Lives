using System;
using UnityEngine;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Flow
{
    public abstract class FlowNode : ScriptableObject, IFlowOutput
    {
        [Header("Node Info")]
        [SerializeField]
        private string nodeId;

        [SerializeField]
        private string displayName;

        [SerializeField]
        private bool isCheckpoint;

        [SerializeField]
        private Vector2 editorPosition;

        [SerializeField]
        private Vector2 editorSize = new(220, 90);

        [SerializeField]
        protected FlowNode nextNode;

        public virtual int OutputCount => 1;

        public string NodeType => GetType().Name;

        public string NodeId => nodeId;
        public string DisplayName => displayName;
        public bool IsCheckpoint => isCheckpoint;
        public Vector2 EditorPosition => editorPosition;

        // Dipanggil ketika node mulai dijalankan.
        public abstract void Enter();

        // Dipanggil jika node perlu dihentikan secara paksa.
        public virtual void Exit()
        {
        }

        // Event ketika node selesai.
        // FlowManager akan mendengarkan event ini.
        public event Action<FlowNode> Completed;

#if UNITY_EDITOR
        public void Initialize()
        {
            if (string.IsNullOrEmpty(nodeId))
            {
                nodeId = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrEmpty(displayName))
            {
                displayName = GetType().Name;
            }

            name = displayName;

            EditorUtility.SetDirty(this);
        }

        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(displayName))
            {
                name = displayName;

                EditorUtility.SetDirty(this);
            }
        }
#endif

        protected void Complete(FlowNode nextNode)
        {
            Completed?.Invoke(nextNode);
        }

        protected void Complete()
        {
            Complete(null);
        }
        public virtual IEnumerable<FlowNode> GetOutputs()
        {
            if (nextNode != null)
                yield return nextNode;
        }

        public virtual void SetOutput(int index, FlowNode node)
        {
            if (index == 0)
                nextNode = node;
        }

        public virtual void RemoveOutput(FlowNode node)
        {
            if (nextNode == node)
                nextNode = null;
        }

        public virtual string GetOutputName(int index)
        {
            return "Next";
        }

#if UNITY_EDITOR
        public void SetEditorPosition(Vector2 position)
        {
            editorPosition = position;

            EditorUtility.SetDirty(this);
        }
#endif
    }
}