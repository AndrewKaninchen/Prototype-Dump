using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GraphAPI
{
    public class MyGraphWindow : EditorWindow
    {
        private MyGraphView m_graph;

        private readonly int m_kMargin = 50;
        private readonly int m_kPadding = 10;
        private readonly int m_kBoxSize = 150;

        [MenuItem("Graph API/MyGraphWindow")]
        public static void ShowExample()
        {
            var wnd = GetWindow<MyGraphWindow>();
            wnd.titleContent = new GUIContent("My Graph");
        }
		
        public void Update()
        {
            m_graph.layout = new Rect (Vector2.zero, position.size);
        }

        public void OnEnable()
        {
            var root = this.GetRootVisualContainer();
            m_graph = new MyGraphView(this);    
            m_graph.RegisterCallback<KeyDownEvent>(OnSpaceDown);

            m_graph.AddManipulator(new ContentDragger());
            m_graph.AddManipulator(new RectangleSelector());
            //graph.AddManipulator(new SelectionDragger());

            m_graph.AddNode(new MyNode(m_graph, Vector2.zero));

            root.Add(m_graph);
        }

        private void OnSpaceDown(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Space)
            {
                var graphViewPos = m_graph.viewTransform.position;
                var pos = evt.imguiEvent.mousePosition - new Vector2(graphViewPos.x, graphViewPos.y);
                m_graph.AddNode(new MyNode(m_graph, pos));
            }
        }

    
    }


    public class MyGraphView : GraphView
    {
        private Grid m_grid;

        public MyGraphView(MyGraphWindow window)
        {
            var grid = new GridBackground();
            Add(grid);
            grid.SendToBack();        
        }

        public void AddNode(MyNode node)
        {
            AddElement(node);
            node.BringToFront();        
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(nap =>
                    nap.direction != startPort.direction &&
                    nap.node != startPort.node)
                //        nodeAdapter.GetAdapter(nap.source, startPort.source) != null)
                .ToList();
        }
    }

    public class MyNode : Node
    {
        private List<Port> m_inputPorts = new List<Port>();
        private List<Port> m_outputPorts = new List<Port>();
        private MyGraphView m_graph;

        public MyNode (MyGraphView graph, Vector2 pos) : base()
        {
            m_graph = graph;

            SetPosition(new Rect(pos, GetPosition().size));
            //layout = new Rect(pos, size);

            this.AddManipulator(new Dragger());

            AddPort(Orientation.Horizontal, Direction.Input);
            AddPort(Orientation.Horizontal, Direction.Output);
            AddPort(Orientation.Horizontal, Direction.Output);
            AddPort(Orientation.Horizontal, Direction.Output);        
        }

        public void AddPort(Orientation orientation, Direction direction)
        {
            var port = MyGraphPort.Create(orientation, direction, Port.Capacity.Single, typeof(int), new MyEdgeConnectorListener(m_graph));

            if (direction == Direction.Input)
            {
                m_inputPorts.Add(port);
                inputContainer.Add(port);
            }
            else
            {
                m_outputPorts.Add(port);
                outputContainer.Add(port);
            }        
        }

        private class MyGraphPort : Port
        {
            private MyGraphPort(Orientation portOrientation, Direction portDirection, Capacity capacity, Type type) :
                base(portOrientation, portDirection, capacity, type)
            {
            }

            public static MyGraphPort Create(Orientation portOrientation, Direction portDirection, Capacity capacity, Type type, IEdgeConnectorListener connectorListener)
            {
                var port = new MyGraphPort(portOrientation, portDirection, capacity, type) { m_EdgeConnector = new EdgeConnector<Edge> (connectorListener) };            
                port.AddManipulator(port.m_EdgeConnector);
                port.portType = type;
                return port;
            }
        }

        private class MyEdgeConnectorListener : IEdgeConnectorListener
        {
            private MyGraphView m_graph;

            public MyEdgeConnectorListener(MyGraphView graph)
            {
                m_graph = graph;
            }



            public void OnDropOutsidePort(Edge edge, Vector2 position)
            {
            
                Debug.Log("OnDropOutsidePort");

                //var draggedPort = (edge.output != null ? edge.output.edgeConnector.edgeDragHelper.draggedPort : null) ?? (edge.input != null ? edge.input.edgeConnector.edgeDragHelper.draggedPort : null);
                //m_SearchWindowProvider.connectedPort = (ShaderPort)draggedPort;
                //SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), m_SearchWindowProvider);
            }

            public void OnDrop(GraphView graphView, Edge edge)
            {
                Debug.Log("OnDrop");
                edge.input.ConnectTo(edge.output);
                m_graph.AddElement(edge);
            
            }
        }

        private class MyEdge : Edge
        {

        }
    }
}