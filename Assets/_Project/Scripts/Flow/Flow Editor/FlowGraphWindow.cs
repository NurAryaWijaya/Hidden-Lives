using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace Game.Flow.Editor
{
    public class FlowGraphWindow : EditorWindow
    {
        private FlowGraph currentGraph;

        private FlowCanvas canvas;

        [MenuItem("Window/Hidden Live/Flow Graph")]
        public static void Open()
        {
            GetWindow<FlowGraphWindow>("Flow Graph");
        }

        private void CreateCanvas()
        {
            canvas = new FlowCanvas();

            canvas.CreateNodeRequested += CreateNode;

            canvas.NodeDeleted += RefreshGraph;

            canvas.focusable = true;
            canvas.RegisterCallback<KeyDownEvent>(OnKeyDown);

            rootVisualElement.Add(canvas);
        }

        private void OnEnable()
        {
            CreateToolbar();

            CreateCanvas();
        }

        private void OnDisable()
        {
            rootVisualElement.Clear();

            canvas = null;

            currentGraph = null;
        }

        public void OpenGraph(FlowGraph graph)
        {
            if (graph == null)
                return;

            currentGraph = graph;

            ReloadGraph();
        }

        private void ReloadGraph()
        {
            Debug.Log($"Reload Graph : {currentGraph.DisplayName}");
            Debug.Log($"Node Count : {currentGraph.Nodes.Count}");
            if (currentGraph == null)
                return;

            canvas.ClearNodes();

            canvas.SetGraph(currentGraph);

            foreach (FlowNode node in currentGraph.Nodes)
            {
                FlowNodeView nodeView = new FlowNodeView(node);

                canvas.AddNode(nodeView);
            }

            canvas.BuildConnections();
        }

        public void RefreshGraph()
        {
            if (currentGraph == null)
                return;

            ReloadGraph();
        }

        private void CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();

            ObjectField graphField = new ObjectField("Flow Graph")
            {
                objectType = typeof(FlowGraph)
            };

            graphField.RegisterValueChangedCallback(evt =>
            {
                OpenGraph(evt.newValue as FlowGraph);
            });

            toolbar.Add(graphField);

            rootVisualElement.Add(toolbar);
        }

        private void CreateNode(Type nodeType, Vector2 position)
        {
            if (currentGraph == null)
                return;

            FlowNode node = CreateInstance(nodeType) as FlowNode;

            node.name = nodeType.Name;

            node.SetEditorPosition(position);

            currentGraph.AddNode(node);

            AssetDatabase.AddObjectToAsset(node, currentGraph);

            EditorUtility.SetDirty(node);
            EditorUtility.SetDirty(currentGraph);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            ReloadGraph();
        }

        private void OnKeyDown(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Delete)
            {
                canvas.DeleteSelectedNode();

                evt.StopPropagation();
            }
        }
    }
}
