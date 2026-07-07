using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.CameraUI;

namespace Game.Flow.Editor
{
    public class FlowCanvas : VisualElement
    {
        private readonly VisualElement background;
        private readonly VisualElement content;

        // Connection
        private readonly VisualElement connectionLayer;
        private readonly VisualElement nodeLayer;

        // Grid
        private readonly FlowGrid grid;

        // Selction
        private FlowNodeView selectedNode;
        private FlowGraph graph;

        // Pan
        private bool isPanning;
        private Vector2 lastMousePosition;

        // View Transform
        private readonly FlowViewTransform viewTransform = new();

        // Port
        private FlowPortView draggingPort;
        private FlowConnectionPreview previewConnection;
        private Vector2 currentMousePosition;

        // Menu
        private Vector2 contextMenuPosition;
        public event Action<Vector2> CreateTestNodeRequested;
        public event Action NodeDeleted;

        public FlowViewTransform ViewTransform => viewTransform;

        private readonly Dictionary<FlowNode, FlowNodeView> nodeMap = new();
        private readonly List<FlowConnectionView> connections = new();
        public FlowCanvas()
        {
            name = "FlowCanvas";
            style.flexGrow = 1;
            style.overflow = Overflow.Hidden;

            // Background
            background = new VisualElement();
            background.style.position = Position.Absolute;
            background.style.left = 0;
            background.style.right = 0;
            background.style.top = 0;
            background.style.bottom = 0;
            background.style.backgroundColor = new Color(0.17f, 0.17f, 0.17f);

            Add(background);

            // Grid
            grid = new FlowGrid(viewTransform);
            Add(grid);

            // Zoom
            RegisterCallback<WheelEvent>(OnWheel);

            // Connection Layer
            connectionLayer = new VisualElement();

            connectionLayer.style.position = Position.Absolute;
            connectionLayer.style.left = 0;
            connectionLayer.style.right = 0;
            connectionLayer.style.top = 0;
            connectionLayer.style.bottom = 0;

            Add(connectionLayer);
            // Port Layer
            previewConnection = new FlowConnectionPreview();
            connectionLayer.Add(previewConnection);

            // Node Layer
            nodeLayer = new VisualElement();

            nodeLayer.style.position = Position.Absolute;
            nodeLayer.style.left = 0;
            nodeLayer.style.right = 0;
            nodeLayer.style.top = 0;
            nodeLayer.style.bottom = 0;

            Add(nodeLayer);

            RegisterCallback<MouseDownEvent>(OnMouseDown);
            RegisterCallback<MouseMoveEvent>(OnMouseMove);
            RegisterCallback<MouseUpEvent>(OnMouseUp, TrickleDown.TrickleDown);

            RegisterCallback<ContextClickEvent>(OnContextClick);
        }

        // GRAPH SETUP
        public void SetGraph(FlowGraph graph)
        {
            this.graph = graph;
        }

        // NODE
        public void AddNode(FlowNodeView nodeView)
        {
            nodeView.Selected += HandleNodeSelected;

            nodeMap[nodeView.Node] = nodeView;
            nodeLayer.Add(nodeView);

            foreach (var port in nodeView.OutputPorts)
            {
                port.DragStarted += BeginConnection;
            }
        }

        public void ClearNodes()
        {
            foreach (FlowNodeView node in nodeMap.Values)
            {
                foreach (FlowPortView port in node.OutputPorts)
                {
                    port.DragStarted -= BeginConnection;
                }
            }

            nodeLayer.Clear();
            nodeMap.Clear();
        }

        // SELECTION
        private void HandleNodeSelected(FlowNodeView nodeView)
        {
            if (selectedNode != null)
                selectedNode.SetSelected(false);

            selectedNode = nodeView;
            selectedNode.SetSelected(true);

            Focus();

#if UNITY_EDITOR
            UnityEditor.Selection.activeObject = nodeView.Node;
#endif
        }

        // Connection
        public void AddConnection(FlowConnectionView connection)
        {
            connections.Add(connection);
            connectionLayer.Add(connection);
        }

        public void ClearConnections()
        {
            foreach (var c in connections)
            {
                connectionLayer.Remove(c);
            }

            connections.Clear();
        }

        public void BuildConnections()
        {
            ClearConnections();

            foreach (FlowNode node in graph.Nodes)
            {
                if (node is not IFlowOutput output)
                    continue;

                if (!nodeMap.TryGetValue(node, out FlowNodeView fromView))
                    continue;

                List<FlowNode> outputs = output.GetOutputs().ToList();

                for (int i = 0; i < outputs.Count; i++)
                {
                    FlowNode target = outputs[i];

                    if (target == null)
                        continue;

                    if (!nodeMap.TryGetValue(target, out FlowNodeView toView))
                        continue;

                    if (i >= fromView.OutputPorts.Count)
                        continue;

                    AddConnection(
                        new FlowConnectionView(
                            fromView.OutputPorts[i],
                            toView.InputPort));
                }
            }
        }

        // Pan
        private void OnMouseDown(MouseDownEvent evt)
        {
            // Middle Mouse
            if (evt.button != 2)
                return;

            isPanning = true;
            lastMousePosition = evt.mousePosition;

            evt.StopPropagation();
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            currentMousePosition = evt.mousePosition;

            if (isPanning)
            {
                Vector2 delta = evt.mousePosition - lastMousePosition;

                lastMousePosition = evt.mousePosition;

                viewTransform.Position += delta;

                ApplyViewTransform();
            }

            if (draggingPort != null)
            {
                Vector2 localPosition =
                    connectionLayer.WorldToLocal(evt.mousePosition);

                UpdateConnection(localPosition);
            }
        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            // selesai pan
            if (evt.button == 2)
            {
                isPanning = false;
                evt.StopPropagation();
                return;
            }

            // selesai drag connection
            if (evt.button == 0 && draggingPort != null)
            {
                TryCompleteConnection(draggingPort);
                EndConnection();

                evt.StopPropagation();
            }
        }

        private void OnContextClick(ContextClickEvent evt)
        {
            contextMenuPosition = ScreenToCanvasPosition(currentMousePosition);

            GenericMenu menu = new GenericMenu();

            menu.AddItem(
                new GUIContent("Create Node/Test Flow Node"),
                false,
                () =>
                {
                    CreateTestNodeRequested?.Invoke(contextMenuPosition);
                });

            menu.ShowAsContext();

            evt.StopPropagation();
        }

        private void ApplyViewTransform()
        {
            // Grid tidak ikut digeser.
            // Grid hanya menggambar berdasarkan ViewTransform.
            grid.MarkDirtyRepaint();

            nodeLayer.style.translate =
                new Translate(
                    viewTransform.Position.x,
                    viewTransform.Position.y);

            nodeLayer.style.scale =
                new Scale(new Vector3(
                    viewTransform.Scale,
                    viewTransform.Scale,
                    1));

            foreach (var node in nodeMap.Values)
            {
                node.SetZoomScale(viewTransform.Scale);
            }

            connectionLayer.style.translate =
                new Translate(0, 0);

            connectionLayer.style.scale =
                new Scale(Vector3.one);

            grid.Refresh();
        }

        private Vector2 ScreenToCanvasPosition(Vector2 screenPosition)
        {
            Vector2 position = screenPosition;

            position -= viewTransform.Position;

            position /= viewTransform.Scale;

            return position;
        }

        // Zoom
        private void OnWheel(WheelEvent evt)
        {
            float zoomSpeed = 0.1f;

            if (evt.delta.y < 0)
                viewTransform.Scale += zoomSpeed;
            else
                viewTransform.Scale -= zoomSpeed;

            viewTransform.Scale = Mathf.Clamp(
                viewTransform.Scale,
                0.25f,
                2.5f);

            ApplyViewTransform();

            evt.StopPropagation();
        }

        // Ports
        public void BeginConnection(FlowPortView port)
        {
            draggingPort = port;

            previewConnection.SetStart(port);

            previewConnection.SetVisible(true);

            Focus();
        }

        public void UpdateConnection(Vector2 mousePosition)
        {
            if (draggingPort == null)
                return;

            Vector2 canvasPosition =
                ScreenToCanvasPosition(mousePosition);

            previewConnection.SetEnd(canvasPosition);
        }

        public void EndConnection()
        {
            previewConnection.SetVisible(false);

            draggingPort = null;
        }

        private void TryCompleteConnection(FlowPortView fromPort)
        {
            FlowPortView target = FindInputPortUnderMouse();

            if (target != null)
            {
                Connect(fromPort, target);
            }

            EndConnection();
        }

        private FlowPortView FindInputPortUnderMouse()
        {
            foreach (var node in nodeMap.Values)
            {
                if (node.InputPort.worldBound.Contains(currentMousePosition))
                {
                    return node.InputPort;
                }
            }

            return null;
        }

        private void Connect(FlowPortView fromPort, FlowPortView toPort)
        {
            Debug.Log(
                $"{fromPort.Owner.Node.name} -> {toPort.Owner.Node.name}");

            if (fromPort.Owner.Node is TestFlowNode fromNode)
            {
                fromNode.SetNextNode(toPort.Owner.Node);

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(fromNode);
#endif

                BuildConnections();
            }
        }

        // Delete Node
        public void DeleteSelectedNode()
        {
            if (selectedNode == null)
                return;

            foreach (FlowNode node in graph.Nodes)
            {
                if (node is IFlowOutput output)
                {
                    output.RemoveOutput(selectedNode.Node);
                }
            }

            graph.RemoveNode(selectedNode.Node);

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.RemoveObjectFromAsset(selectedNode.Node);

            UnityEditor.EditorUtility.SetDirty(graph);

            UnityEditor.AssetDatabase.SaveAssets();
#endif

            selectedNode = null;

            NodeDeleted?.Invoke();

            BuildConnections();
        }
    }
}