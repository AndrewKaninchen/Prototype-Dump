using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RealmWorks
{
    public class ContentWindow : EditorWindow
    {
        [SerializeField] private TreeViewState m_TreeViewState;
        private SimpleTreeView m_SimpleTreeView;
        private Texture m_Uchan;

        [SerializeField] private Dictionary<string, bool> m_CategoryFolds;

        [MenuItem("RealmWorks/Content")]
        public static void GetWindow()
        {
            var wnd = GetWindow<ContentWindow>();
            wnd.titleContent = new GUIContent("Content");
        }

        private void OnEnable()
        {
            m_Uchan = EditorGUIUtility.Load("uchan.png") as Texture;

            if (m_TreeViewState == null)
                m_TreeViewState = new TreeViewState();

            m_SimpleTreeView = new SimpleTreeView(m_TreeViewState);

            m_CategoryFolds = new Dictionary<string, bool>
            {
                {"People", false},
                {"Groups", false},
                {"Places", false},
                {"Things", false}
            };

        }

        private Vector2 m_Scroll = Vector2.zero;
        public void OnGUI()
        {
            var style = new GUIStyle(EditorStyles.helpBox)
            {
                richText = true,
                fontSize = 11
            };

            //style. = 18;
            m_Scroll = EditorGUILayout.BeginScrollView(m_Scroll);
            
            CategoryBar("People", Sirenix.Utilities.Editor.EditorIcons.SingleUser.Raw);
            CategoryBar("Groups", Sirenix.Utilities.Editor.EditorIcons.MultiUser.Raw);
            CategoryBar("Places", Sirenix.Utilities.Editor.EditorIcons.Globe.Raw);
            CategoryBar("Things", Sirenix.Utilities.Editor.EditorIcons.Tree.Raw);
            //CategoryBar("Plots");
            
            EditorGUILayout.EndScrollView();
        }
        
        private void CategoryBar(string name, Texture2D texture)
        {
            m_CategoryFolds[name] = CategoryBar(name, m_CategoryFolds[name], texture);
        }

        private bool CategoryBar(string name, bool fold, Texture2D texture)
        {
            bool internalFold;
            using (var h = new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                internalFold = GUI.Toggle(h.rect, fold, "", GUIStyle.none);                

                const float iconSize = 30f;                
                var iconRect = GUILayoutUtility.GetRect(iconSize, iconSize, new GUIStyle() { stretchWidth = false });                

                //GUI.Box(iconRect, "");
                GUI.DrawTexture(iconRect, texture);

                var labelStyle = new GUIStyle (EditorStyles.label)
                {
                    richText = true,
                    fontSize = 11,                    
                    stretchWidth = false,
                    alignment = TextAnchor.MiddleLeft                    
                };

                var labelRect = GUILayoutUtility.GetRect(new GUIContent("<b>   " + name + "</b>"), labelStyle);
                labelRect.height = iconSize;
                //GUI.Box(labelRect, "");

                GUI.Label(labelRect, "<b>   " + name + "</b>", labelStyle);

                var emptyRect = GUILayoutUtility.GetRect(new GUIContent(""), GUIStyle.none);
                emptyRect.height = iconSize;

                //GUI.Box(emptyRect, "");

                const int size = 15;
                var triangleRect = new Rect(emptyRect.xMax - size, emptyRect.y, size, size);
                triangleRect.center = new Vector2(triangleRect.x, emptyRect.center.y);

                //GUI.Box(triangleRect, "");

                GUI.Label(triangleRect, fold ? "\u25BC" : "\u25B6", new GUIStyle(labelStyle) { alignment = TextAnchor.MiddleCenter});
            }
            
            if (fold)
            {
                //EditorGUI.indentLevel++;                
                var rect = GUILayoutUtility.GetRect(position.width - 30f, m_SimpleTreeView.totalHeight);
                rect.position += new Vector2(10f, 0f);
                //rect = EditorGUI.IndentedRect(rect);

                m_SimpleTreeView.OnGUI(rect);
                //EditorGUI.indentLevel--;
            }
            
            return internalFold;
        }

    }

    public class SimpleTreeView : TreeView
    {
        public SimpleTreeView(TreeViewState treeViewState)
            : base(treeViewState)
        {
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            // BuildRoot is called every time Reload is called to ensure that TreeViewItems 
            // are created from data. Here we create a fixed set of items. In a real world example,
            // a data model should be passed into the TreeView and the items created from the model.

            // This section illustrates that IDs should be unique. The root item is required to 
            // have a depth of -1, and the rest of the items increment from that.
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            var allItems = new List<TreeViewItem>
        {
            new TreeViewItem {id = 1, depth = 0, displayName = "Onessa"},
            new TreeViewItem {id = 2, depth = 1, displayName = "Gelaswin"},
            new TreeViewItem {id = 3, depth = 2, displayName = "Waedith"},
            new TreeViewItem {id = 4, depth = 1, displayName = "Toulion"},
            new TreeViewItem {id = 5, depth = 2, displayName = "Crelawen"},
            new TreeViewItem {id = 6, depth = 2, displayName = "Frameer"},
            new TreeViewItem {id = 7, depth = 1, displayName = "Mereena"},
            new TreeViewItem {id = 8, depth = 2, displayName = "White Harbor"},
            new TreeViewItem {id = 9, depth = 1, displayName = "Sam Blackshoe"},
            new TreeViewItem {id = 10, depth = 0, displayName = "Shipwreck Isle"},
            new TreeViewItem {id = 11, depth = 1, displayName = "Haramee Village"},
            new TreeViewItem {id = 12, depth = 2, displayName = "Haramee"},
            new TreeViewItem {id = 13, depth = 1, displayName = "Kobold Ruins"},
            new TreeViewItem {id = 14, depth = 1, displayName = "The Hideout"},
        };

            // Utility method that initializes the TreeViewItem.children and .parent for all items.
            SetupParentsAndChildrenFromDepths(root, allItems);

            // Return root of the tree
            return root;
        }

        protected override bool CanStartDrag(CanStartDragArgs args)
        {
            return true;
        }

        protected override void SetupDragAndDrop(SetupDragAndDropArgs args)
        {
            DragAndDrop.PrepareStartDrag();

            DragAndDrop.StartDrag("");
        }

        protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args)
        {
            return DragAndDropVisualMode.Move;
        }
    }
}