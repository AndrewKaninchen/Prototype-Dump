using System;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace RealmWorks.StoryView.Editor
{
    public class StoryNode : Node
    {
        [SerializeField] private List<Port> m_InputPorts = new List<Port>();
        private List<Port> m_OutputPorts = new List<Port>();
        private StoryGraphView m_Graph;
        private string m_Text;

        private VisualContainer m_OutputPortBox;
        private VisualContainer m_InputPortBox;
        
        

        public StoryNode (StoryGraphView graph, Vector2 pos, string title, string text) : base()
        {
            this.title = title;
            this.m_Text = text;
            m_Graph = graph;
            SetPosition(new Rect(pos, GetPosition().size));
            //layout = new Rect(pos, size);

//            AddPort(Orientation.Horizontal, Direction.Input, "");
//            AddPort(Orientation.Horizontal, Direction.Input, "");
//            AddPort(Orientation.Horizontal, Direction.Output, "");
            

            var textBox = new TextField()
            {
                name = "textBox", 
                persistenceKey = "input",
                multiline = true,                
            };
            
            textBox.style.backgroundColor = extensionContainer.style.backgroundColor;
            extensionContainer.style.backgroundColor = new Color(.2f, .2f, .2f);
//            var ext = new ScrollView();
//            ext.showHorizontal = false;
//            ext.showVertical = true;
            
//            ext.Add(textBox);
            extensionContainer.Add(textBox);
            textBox.SetValueAndNotify(m_Text);
            RefreshExpandedState();
            //capabilities |= Capabilities.Resizable;
            //style.positionType = PositionType.Manual;
            //Add(new StoryNodeResizer());
            
            AddMyPorts();
            contentContainer.AddStyleSheetPath("nodeStyles");
            //contentContainer.Add();
        }

        public void AddMyPorts()
        {
            m_InputPortBox = new VisualContainer()
            {
                name = "input-port-box"
            };
            
            m_OutputPortBox = new VisualContainer()
            {
                name = "output-port-box"
            };


            m_InputPortBox.AddToClassList("port-box");
            m_OutputPortBox.AddToClassList("port-box");
            
            contentContainer.AddToClassList("center-content-vertically");
            
            contentContainer.Add(m_InputPortBox);
            contentContainer.Add(m_OutputPortBox);
            
            AddPort(Orientation.Horizontal, Direction.Input, "");
            AddPort(Orientation.Horizontal, Direction.Output, "");

        }
        
        public void AddPort(Orientation orientation, Direction direction, string text)
        {
            var port = MyGraphPort.Create(orientation, direction, Port.Capacity.Single, typeof(int), new MyEdgeConnectorListener(m_Graph));
            port.portName = "";
            port.AddToClassList("port");
            //port.
            
            if (direction == Direction.Input)
            {
                m_InputPorts.Add(port);
                m_InputPortBox.Add(port);
                port.AddToClassList("input-port");
            }
            else
            {
                m_OutputPorts.Add(port);
                m_OutputPortBox.Add(port);
                port.AddToClassList("output-port");
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
            private StoryGraphView m_Graph;

            public MyEdgeConnectorListener(StoryGraphView graph)
            {
                m_Graph = graph;
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
                m_Graph.AddElement(edge);
            
            }
        }

        private class MyEdge : Edge
        {

        }
    }
}