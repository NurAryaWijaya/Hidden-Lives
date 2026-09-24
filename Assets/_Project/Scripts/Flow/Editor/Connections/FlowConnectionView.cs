using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Flow.Editor
{
    public class FlowConnectionView : ImmediateModeElement
    {
        private readonly FlowPortView fromPort;
        private readonly FlowPortView toPort;

        public FlowPortView FromPort => fromPort;
        public FlowPortView ToPort => toPort;

        public FlowConnectionView(
            FlowPortView fromPort,
            FlowPortView toPort)
        {
            this.fromPort = fromPort;
            this.toPort = toPort;

            pickingMode = PickingMode.Ignore;

            style.position = Position.Absolute;
            style.left = 0;
            style.right = 0;
            style.top = 0;
            style.bottom = 0;
        }

        protected override void ImmediateRepaint()
        {
            if (fromPort == null || toPort == null)
                return;

            DrawConnection();
        }

        private void DrawConnection()
        {
            Vector2 start = fromPort.GetLocalCenter(parent);
            Vector2 end = toPort.GetLocalCenter(parent);

            Vector2 startTangent = start + Vector2.right * 80f;
            Vector2 endTangent = end + Vector2.left * 80f;

            Handles.BeginGUI();

            Handles.DrawBezier(
                start,
                end,
                startTangent,
                endTangent,
                Color.white,
                null,
                3f);

            Handles.EndGUI();
        }

        public void Refresh()
        {
            MarkDirtyRepaint();
        }
    }
}