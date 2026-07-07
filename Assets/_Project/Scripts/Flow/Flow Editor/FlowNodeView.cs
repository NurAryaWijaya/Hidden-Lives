using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Flow.Editor
{
    public class FlowNodeView : VisualElement
    {
        public const float DefaultWidth = 220f;
        public const float DefaultHeight = 90f;

        private readonly Label titleLabel;
        private readonly Label idLabel;

        // Drag
        private bool isDragging;
        private Vector2 dragOffset;
        private float currentScale = 1f;

        //Selection
        public event Action<FlowNodeView> Selected;
        private bool isSelected;

        // Port
        public FlowPortView InputPort { get; }
        public List<FlowPortView> OutputPorts { get; } = new();

        public FlowNode Node { get; }

        public FlowNodeView(FlowNode node)
        {
            Node = node;

            name = "FlowNode";

            // Port
            InputPort = new FlowPortView(this, FlowPortType.Input);

            Add(InputPort);

            if (node is IFlowOutput output)
            {
                int count = output.GetOutputs().Count();

                // Jika belum ada output sama sekali,
                // tetap buat satu port untuk testing.
                if (count == 0)
                    count = 1;

                for (int i = 0; i < count; i++)
                {
                    FlowPortView port = new FlowPortView(
                        this,
                        FlowPortType.Output,
                        i);

                    OutputPorts.Add(port);

                    Add(port);
                }
            }

            // Layout
            style.position = Position.Absolute;

            style.width = DefaultWidth;
            style.height = DefaultHeight;

            style.left = node.EditorPosition.x;
            style.top = node.EditorPosition.y;

            // Background
            style.backgroundColor = new Color(0.22f, 0.22f, 0.22f);

            style.borderTopWidth = 2;
            style.borderBottomWidth = 2;
            style.borderLeftWidth = 2;
            style.borderRightWidth = 2;

            style.borderTopColor = new Color(.35f, .35f, .35f);
            style.borderBottomColor = new Color(.35f, .35f, .35f);
            style.borderLeftColor = new Color(.35f, .35f, .35f);
            style.borderRightColor = new Color(.35f, .35f, .35f);

            // Title
            titleLabel = new Label(node.DisplayName);

            titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            titleLabel.style.marginTop = 8;
            titleLabel.style.marginLeft = 8;

            Add(titleLabel);

            // Node ID
            idLabel = new Label(node.NodeId);

            idLabel.style.marginLeft = 8;
            idLabel.style.marginTop = 4;

            idLabel.style.fontSize = 10;

            idLabel.style.color = new Color(.7f, .7f, .7f);

            Add(idLabel);

            RegisterCallback<GeometryChangedEvent>(_ =>
            {
                RefreshPorts();
            });

            RegisterCallback<MouseDownEvent>(OnMouseDown);
            RegisterCallback<MouseMoveEvent>(OnMouseMove);
            RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (evt.button != 0)
                return;

            isDragging = true;

            dragOffset = evt.localMousePosition;

            Selected?.Invoke(this);

            evt.StopPropagation();
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (!isDragging)
                return;

            Vector2 delta =
                (evt.localMousePosition - dragOffset)
                / currentScale;

            Vector2 pos = new Vector2(
                resolvedStyle.left + delta.x,
                resolvedStyle.top + delta.y);

            style.left = pos.x;
            style.top = pos.y;

            Node.SetEditorPosition(pos);
        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            if (evt.button != 0)
                return;

            isDragging = false;

            evt.StopPropagation();
        }

        // Zoom
        public void SetZoomScale(float scale)
        {
            currentScale = scale;
        }

        // Selected
        public void SetSelected(bool selected)
        {
            isSelected = selected;

            Color borderColor = selected
                ? new Color(0.2f, 0.55f, 1f)
                : new Color(0.35f, 0.35f, 0.35f);

            style.borderTopColor = borderColor;
            style.borderBottomColor = borderColor;
            style.borderLeftColor = borderColor;
            style.borderRightColor = borderColor;

            style.borderTopWidth = selected ? 3 : 2;
            style.borderBottomWidth = selected ? 3 : 2;
            style.borderLeftWidth = selected ? 3 : 2;
            style.borderRightWidth = selected ? 3 : 2;
        }

        // Memperbarui posisi view berdasarkan FlowNode.
        public virtual void RefreshPosition()
        {
            style.left = Node.EditorPosition.x;
            style.top = Node.EditorPosition.y;

            RefreshPorts();
        }

        // Port
        private void RefreshPorts()
        {
            const float size = FlowPortView.PortSize;

            InputPort.style.left = -size * .5f;
            InputPort.style.top = resolvedStyle.height * .5f - size * .5f;

            if (OutputPorts.Count == 0)
                return;

            float spacing = resolvedStyle.height / (OutputPorts.Count + 1);

            for (int i = 0; i < OutputPorts.Count; i++)
            {
                FlowPortView port = OutputPorts[i];

                port.style.left =
                    resolvedStyle.width - size * .5f;

                port.style.top =
                    spacing * (i + 1) - size * .5f;
            }
        }
    }
}