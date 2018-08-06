using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace RealmWorks.StoryView.Editor
{
    public class StoryGraphViewWindow : EditorWindow
    {
        private StoryGraphView m_Graph;

        private readonly int m_KMargin = 50;
        private readonly int m_KPadding = 10;
        private readonly int m_KBoxSize = 150;

        [MenuItem("RealmWorks/StoryView/StoryGraphView")]
        public static void ShowExample()
        {
            var wnd = GetWindow<StoryGraphViewWindow>();
            wnd.titleContent = new GUIContent("Story Graph");
        }
		
        public void Update()
        {
            m_Graph.layout = new Rect (Vector2.zero, position.size);
        }

        public void OnEnable()
        {
            var root = this.GetRootVisualContainer();
            m_Graph = new StoryGraphView(this);    
            m_Graph.RegisterCallback<KeyDownEvent>(OnSpaceDown);
            root.Add(m_Graph);
        }

        private void OnSpaceDown(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Space)
            {
                var graphViewPos = m_Graph.viewTransform.position;
                var pos = evt.imguiEvent.mousePosition - new Vector2(graphViewPos.x, graphViewPos.y);
                m_Graph.AddNode(new StoryNode(m_Graph, pos, "John arrives", 
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
        private Grid m_Grid;

        public StoryGraphView(StoryGraphViewWindow window)
        {
            var grid = new GridBackground();
            //grid.
            Add(grid);
            grid.SendToBack();
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
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