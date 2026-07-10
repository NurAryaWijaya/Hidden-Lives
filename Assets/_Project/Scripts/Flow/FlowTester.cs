using UnityEngine;

namespace Game.Flow
{
    public class FlowTester : MonoBehaviour
    {
        [SerializeField]
        private FlowGraph graph;

        [SerializeField]
        private Vector2 editorPosition;

        private bool IsStarted;

        public Vector2 EditorPosition => editorPosition;

        private void Start()
        {
            if (!IsStarted)
                FlowManager.Instance.StartFlow(graph);

            IsStarted = true;
        }

#if UNITY_EDITOR
        public void SetEditorPosition(Vector2 position)
        {
            editorPosition = position;
        }
#endif
    }
}