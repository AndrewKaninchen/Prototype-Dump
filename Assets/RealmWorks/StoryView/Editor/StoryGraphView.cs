using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace RealmWorks.StoryView
{
    public class StoryGraphViewWindow : EditorWindow
    {
        private StoryGraphView m_graph;

        private readonly int m_kMargin = 50;
        private readonly int m_kPadding = 10;
        private readonly int m_kBoxSize = 150;

        [MenuItem("RealmWorks/StoryView/StoryGraphView")]
        public static void ShowExample()
        {
            var wnd = GetWindow<StoryGraphViewWindow>();
            wnd.titleContent = new GUIContent("Story Graph");
        }
		
        public void Update()
        {
            m_graph.layout = new Rect (Vector2.zero, position.size);
        }

        public void OnEnable()
        {
            var root = this.GetRootVisualContainer();
            m_graph = new StoryGraphView(this);    
            m_graph.RegisterCallback<KeyDownEvent>(OnSpaceDown);

            m_graph.AddManipulator(new ContentDragger());
            m_graph.AddManipulator(new RectangleSelector());
            //graph.AddManipulator(new SelectionDragger());

            //m_graph.AddNode(new StoryNode(m_graph, Vector2.zero, "John arrives", "Half an hour goes by before Commander Harris returns. He closes the door behind him quickly, as though afraid a loose word might slip inside."));

            root.Add(m_graph);
        }

        private void OnSpaceDown(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Space)
            {
                var graphViewPos = m_graph.viewTransform.position;
                var pos = evt.imguiEvent.mousePosition - new Vector2(graphViewPos.x, graphViewPos.y);
                m_graph.AddNode(new StoryNode(m_graph, pos, "John arrives", 
                    "Half an hour goes by before Commander Harris returns. " + "\n" +
                    "He closes the door behind him quickly, " + "\n" +
                    "Half an hour goes by before Commander Harris returns. " + "\n" +
                    "He closes the door behind him quickly, " + "\n" +
                    "Half an hour goes by before Commander Harris returns. " + "\n" +
                    "He closes the door behind him quickly, " + "\n" +
                    "Half an hour goes by before Commander Harris returns. " + "\n" +
                    "Half an hour goes by before Commander Harris returns. " + "\n" +
                    "Half an hour goes by before Commander Harris returns. " + "\n" +
                    "He closes the door behind him quickly, " + "\n" +
                    "He closes the door behind him quickly, " + "\n" +
                    "He closes the door behind him quickly, " + "\n" +
                    "as though afraid a loose word might slip inside."));
            }
        }
    }

    public class StoryGraphView : GraphView
    {
        private Grid m_grid;

        public StoryGraphView(StoryGraphViewWindow window)
        {
            var grid = new GridBackground();
            //grid.
            Add(grid);
            grid.SendToBack();        
        }

        public void AddNode(StoryNode node)
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
}