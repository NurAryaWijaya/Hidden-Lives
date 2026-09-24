using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Flow.Editor
{
    public class FlowConnectionPreview : ImmediateModeElement
    {
        private Vector2 startPosition;
        private Vector2 endPosition;
        //private bool visible;

        public FlowConnectionPreview()
        {
            pickingMode = PickingMode.Ignore;

            style.position = Position.Absolute;
            style.left = 0;
            style.right = 0;
            style.top = 0;
            style.bottom = 0;
        }

        public void SetStart(FlowPortView port)
        {
            //Debug.Log(parent.name);
            Debug.Log(parent == null ? "NULL" : $"Parent = {parent.GetType().Name}");

            startPosition = port.GetLocalCenter(parent);

            MarkDirtyRepaint();
        }

        public void SetEnd(Vector2 position)
        {
            //Debug.Log(parent.name);

            endPosition = position;

            MarkDirtyRepaint();
        }

        public void SetVisible(bool value)
        {
            visible = value;
            MarkDirtyRepaint();
        }

        protected override void ImmediateRepaint()
        {
            if (!visible)
                return;

            if (startPosition == null)
                return;

            Vector2 start = startPosition;

            Vector2 end = endPosition;

            Vector2 startTangent = start + Vector2.right * 80f;
            Vector2 endTangent = end + Vector2.left * 80f;

            Handles.BeginGUI();

            Handles.DrawBezier(
                start,
                end,
                startTangent,
                endTangent,
                Color.yellow,
                null,
                3f);

            Handles.EndGUI();
        }
    }
}