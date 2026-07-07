using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Flow.Editor
{
    public enum FlowPortType
    {
        Input,
        Output
    }
}

namespace Game.Flow.Editor
{
    public class FlowPortView : VisualElement
    {
        public const float PortSize = 12f;

        public FlowNodeView Owner { get; }

        public FlowPortType PortType { get; }

        public int Index { get; }

        public event Action<FlowPortView> DragStarted;
        //public event Action<FlowPortView> DragEnded;

        public FlowPortView(
            FlowNodeView owner,
            FlowPortType portType,
            int index = 0)
        {
            Owner = owner;
            PortType = portType;
            Index = index;

            style.position = Position.Absolute;

            style.width = PortSize;
            style.height = PortSize;

            style.backgroundColor = Color.white;

            style.borderTopLeftRadius = PortSize;
            style.borderTopRightRadius = PortSize;
            style.borderBottomLeftRadius = PortSize;
            style.borderBottomRightRadius = PortSize;

            pickingMode = PickingMode.Position;

            RegisterCallback<MouseDownEvent>(OnMouseDown);
        }

        // Mengambil posisi tengah port dalam koordinat world UI.
        // Digunakan oleh FlowConnectionView.
        public Vector2 GetLocalCenter(VisualElement relativeTo)
        {
            return this.ChangeCoordinatesTo(
                relativeTo,
                new Vector2(
                    layout.width * 0.5f,
                    layout.height * 0.5f));
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (PortType != FlowPortType.Output)
                return;

            if (evt.button != 0)
                return;

            DragStarted?.Invoke(this);

            evt.StopPropagation();
        }
    }
}