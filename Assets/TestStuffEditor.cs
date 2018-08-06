using DefaultNamespace;
using UnityEditor;
using UnityEngine;
using Sirenix.Utilities.Editor;

//
//[CustomEditor(typeof(TestStuff))]
//public class TestStuffEditor: Editor
//{
//    public override void OnInspectorGUI()
//    {
//        var s1 = serializedObject.FindProperty("m_S1");    
//        var s2 = serializedObject.FindProperty("m_S2");
//        
//        //SirenixEditorGUI.Title("Hello", "MyFriend", TextAlignment.Left, true);
//        SirenixEditorGUI.BeginVerticalList();
//            SirenixEditorGUI.BeginListItem();
//                EditorGUILayout.PropertyField(s1);
//            SirenixEditorGUI.EndListItem();
//            SirenixEditorGUI.BeginListItem();
//                EditorGUILayout.PropertyField(s2);
//            SirenixEditorGUI.EndListItem();
//        SirenixEditorGUI.EndVerticalList();
//    }
//}
