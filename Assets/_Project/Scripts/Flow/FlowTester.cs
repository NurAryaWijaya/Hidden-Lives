using UnityEngine;

namespace Game.Flow
{
    public class FlowTester : MonoBehaviour
    {
        [SerializeField]
        private FlowGraph graph;

        [SerializeField]
        private Vector2 editorPosition;

        public Vector2 EditorPosition => editorPosition;

        private void Start()
        {
            FlowManager.Instance.StartFlow(graph);
        }

#if UNITY_EDITOR
        public void SetEditorPosition(Vector2 position)
        {
            editorPosition = position;
        }
#endif
    }
}