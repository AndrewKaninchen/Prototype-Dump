using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Assertions.Must;

namespace InkDraft
{
    public class InkVariablesWindow : EditorWindow
    {
        [SerializeField] public VariablesScriptableObject m_VariableScriptableObject;
        private ReorderableList m_ReorderableList;
        private SerializedObject m_SerializedObject;
        private SerializedProperty m_VariablesProp;

        [MenuItem("InkDraft/Variables")]
        public static void GetWindow()
        {
            var wnd = GetWindow<InkVariablesWindow>();
            wnd.titleContent = new GUIContent("Variables");
        }
        
        private void OnGUI()
        {
            using (new EditorGUI.IndentLevelScope())
            {
                HandleScriptableObject();
            }
        }

        private void HandleScriptableObject()
        {
            //if (m_VariableScriptableObject == null)
            {
                m_VariableScriptableObject = EditorGUILayout.ObjectField("Scriptable Objectzin", m_VariableScriptableObject, typeof(VariablesScriptableObject), false) as VariablesScriptableObject;
                //return;
            }

                
            if (m_SerializedObject == null)
            {
                m_SerializedObject = new SerializedObject(m_VariableScriptableObject);                
                m_VariablesProp = m_SerializedObject.FindProperty("variables");
                //m_VariablesProp.arra
                ConfigList();

                m_ReorderableList.DoLayoutList();                
                m_ReorderableList.DoList(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect()));
                m_SerializedObject.Update();
            }
            else
            {
                m_ReorderableList.DoLayoutList();
                //m_ReorderableList.DoList(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect()));
                m_SerializedObject.Update();
            }
        }

        private void ConfigList()
        {
            m_ReorderableList = new ReorderableList
            (
                elements: m_VariableScriptableObject.variables,
                elementType: typeof(Variable),
                draggable: true,
                displayAddButton: true,
                displayHeader: false,                
                displayRemoveButton: true
            );

//            m_ReorderableList.drawHeaderCallback += (Rect rect) =>
//            {
//                m_VariableScriptableObject = EditorGUI.ObjectField(rect, "Scriptable Objectzin", m_VariableScriptableObject, typeof(VariablesScriptableObject), false) as VariablesScriptableObject;
//            };
            
            
            m_ReorderableList.elementHeightCallback += (int index) =>
            {
                var count = 0;
                var it = m_VariablesProp.GetArrayElementAtIndex(index).GetEnumerator();
                while (it.MoveNext())
                {
                    count++;
                }

                return (count+1) * m_ReorderableList.elementHeight;
            };
            m_ReorderableList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var property = m_VariablesProp.GetArrayElementAtIndex(index);

                SerializedProperty nameProperty = property.FindPropertyRelative("name");
                SerializedProperty typeProperty = property.FindPropertyRelative("type");
                

                //GUI.Box(rect, "");
                var fullRect = new Rect(rect)
                {
                    height = EditorGUIUtility.singleLineHeight * 3f + EditorGUIUtility.standardVerticalSpacing,
                    center = rect.center
                };

                var nameRect = new Rect(fullRect) {height = EditorGUIUtility.singleLineHeight};

                var typeRect = new Rect(rect)
                {
                    yMin = nameRect.yMax + EditorGUIUtility.standardVerticalSpacing,
                    height = EditorGUIUtility.singleLineHeight,
                };

                var valueRect = new Rect(rect)
                {
                    yMin = typeRect.yMax + EditorGUIUtility.standardVerticalSpacing,
                    height = EditorGUIUtility.singleLineHeight,
                };

                var oldLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 55f;

                EditorGUI.PropertyField(nameRect, nameProperty);

                {
                    var old = typeProperty.enumValueIndex;
                    EditorGUI.PropertyField(typeRect, typeProperty);
                    if (typeProperty.enumValueIndex != old) 
                        UpdateVariableType(index, typeProperty.enumValueIndex, nameProperty.stringValue);
                }
                
                //else
                {
                    switch (m_VariableScriptableObject.variables[index].type)
                    {
                        case Variable.VariableType.Int:
                            var intVar = m_VariableScriptableObject.variables[index] as IntVariable;
                            if (intVar != null) intVar.Value = EditorGUI.IntField(valueRect, "Value", intVar.Value);

                            break;
                        case Variable.VariableType.Bool:
                            var boolVar = m_VariableScriptableObject.variables[index] as BoolVariable;
                            if (boolVar != null) boolVar.Value = EditorGUI.Toggle(valueRect, "Value", boolVar.Value);

                            break;
                        case Variable.VariableType.GameObject:
                            var goVar = m_VariableScriptableObject.variables[index] as GameObjectVariable;
                            if (goVar != null) goVar.Value = EditorGUI.ObjectField(valueRect, "Value", goVar.Value, typeof(GameObject), true) as GameObject;

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                    //var valueProperty = property.FindPropertyRelative("value");
                    //Debug.Log(valueProperty);
                    //EditorGUI.PropertyField(valueRect, valueProperty);
                }

                EditorGUIUtility.labelWidth = oldLabelWidth;
                nameProperty.serializedObject.ApplyModifiedProperties();
            };

            m_ReorderableList.onAddCallback += (ReorderableList list) =>
            {
                m_VariableScriptableObject.variables.Add(Variable.CreateInstance(Variable.VariableType.Bool, "New Variable"));
            };

            m_ReorderableList.onRemoveCallback += (ReorderableList list) =>
            {
                m_VariableScriptableObject.variables.RemoveAt(list.index);
            };
        }

        private void UpdateVariableType(int index, int typePropertyEnumValueIndex, string variableName)
        {
            m_VariableScriptableObject.variables[index] = Variable.CreateInstance((Variable.VariableType) typePropertyEnumValueIndex, variableName);
        }
    }
}