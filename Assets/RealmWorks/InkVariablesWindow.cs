using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace InkDraft
{
    public class InkVariables : EditorWindow
    {
        [SerializeField] public VariablesScriptableObject m_VariableScriptableObject;
        private ReorderableList m_ReorderableList;
        private SerializedObject m_SerializedObject;
        private SerializedProperty m_VariablesProp;

        [MenuItem("InkDraft/Variables")]
        public static void GetWindow()
        {
            var wnd = GetWindow<InkVariables>();
            wnd.titleContent = new GUIContent("Variables");
        }
        
        private void OnGUI()
        {
            m_VariableScriptableObject = EditorGUILayout.ObjectField("Scriptable Objectzin", m_VariableScriptableObject, typeof(VariablesScriptableObject), false) as VariablesScriptableObject;

            if (m_VariableScriptableObject == null) return;
            
            if (m_SerializedObject == null)
            {
                m_SerializedObject = new SerializedObject(m_VariableScriptableObject);                
                m_VariablesProp = m_SerializedObject.FindProperty("m_Variables");
                ConfigList();
                
                m_ReorderableList.DoLayoutList();
                m_SerializedObject.Update();
            }
            else
            {
                m_ReorderableList.DoLayoutList();
                m_SerializedObject.Update();
            }
        }

        private void ConfigList()
        {
            m_ReorderableList = new ReorderableList
            (
                elements: m_VariableScriptableObject.m_Variables, 
                elementType: typeof(Variable),
                draggable: true,
                displayAddButton: true,
                displayHeader: false,                
                displayRemoveButton: true
            );

            m_ReorderableList.drawHeaderCallback += (Rect rect) =>
            {
                //GUI.Label(rect, "Variables");
            };
            
            m_ReorderableList.elementHeightCallback += (int index) =>
            {
                var count = 0;
                var it = m_VariablesProp.GetArrayElementAtIndex(index).GetEnumerator();
                while (it.MoveNext())
                {
                    count++;
                }

                return count * m_ReorderableList.elementHeight;
            };
            m_ReorderableList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var property = m_VariablesProp.GetArrayElementAtIndex(index);
                
                SerializedProperty nameProperty = property.FindPropertyRelative("m_Name");
                SerializedProperty typeProperty = property.FindPropertyRelative("m_Type");                
                
                //GUI.Box(rect, "");
                var fullRect = new Rect(rect)
                {
                    height = EditorGUIUtility.singleLineHeight * 2f + EditorGUIUtility.standardVerticalSpacing,
                    center = rect.center
                };
            
                var nameRect = new Rect(fullRect) { height = EditorGUIUtility.singleLineHeight};
            
                var typeRect = new Rect(rect)
                {
                    yMin = nameRect.yMax + EditorGUIUtility.standardVerticalSpacing,
                    height = EditorGUIUtility.singleLineHeight,
                };

                var oldLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 55f;
                
                EditorGUI.PropertyField(nameRect, nameProperty/*, new GUIContent("")*/);
                EditorGUI.PropertyField(typeRect, typeProperty/*, new GUIContent("")*/);
                
                EditorGUIUtility.labelWidth = oldLabelWidth;
                
                nameProperty.serializedObject.ApplyModifiedProperties();
            };

            m_ReorderableList.onAddCallback += (ReorderableList list) =>
            {
                m_VariableScriptableObject.m_Variables.Add(new Variable("New Variable", "int"));
            };

            m_ReorderableList.onRemoveCallback += (ReorderableList list) =>
            {
                m_VariableScriptableObject.m_Variables.RemoveAt(list.index);
            };
        }
    }
}