using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditorInternal;


[CustomEditor(typeof(ComponentEmbedder))]
public class ComponentEmbedderEditor : Editor
{
    private ComponentEmbedder componentEmbedder;
	private Dictionary<Component, Editor> editors = new Dictionary<Component, Editor>();
	private Dictionary<Component, bool> folds = new Dictionary<Component, bool>();
    private ReorderableList reorderableList;
    private void OnEnable()
    {
        var toEmbedListProperty = serializedObject.FindProperty("toEmbed");
        reorderableList = new ReorderableList
        (  
            serializedObject: serializedObject,
            elements: toEmbedListProperty,
            draggable: true,
            displayAddButton: true,
            displayHeader: true,
            displayRemoveButton: true
        );

        reorderableList.drawHeaderCallback += (Rect rect) =>
        {
            GUI.Label(rect, "Components to Embed");
        };

        reorderableList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var propRect = new Rect(rect)
            {
                height = EditorGUIUtility.singleLineHeight,
                center = rect.center
            };
            EditorGUI.PropertyField(propRect, toEmbedListProperty.GetArrayElementAtIndex(index), label: new GUIContent());
        };

        reorderableList.onAddCallback += (ReorderableList list) =>
        {
            componentEmbedder.toEmbed.Add(null);
        };

        reorderableList.onRemoveCallback += (ReorderableList list) =>
        {            
            componentEmbedder.toEmbed.RemoveAt(list.index);
        };       
    }   

    public override void OnInspectorGUI()
    {
		componentEmbedder = target as ComponentEmbedder;
        EditorGUILayout.Space();
        reorderableList.DoLayoutList();	

		RefreshComponentEditorCache();

		foreach (var editor in editors)
		{
			try
			{
				using (var v = new EditorGUILayout.VerticalScope())
					DrawComponentEditor(editor.Key, v);
				EditorGUILayout.Space();
			}
			catch {}
		}

        serializedObject.ApplyModifiedProperties();
    }

	private void ClearComponentEditorCache()
	{
        foreach (var ed in editors)
        {
            RemoveEmbeddedComponent(ed.Key);
        }
		editors.Clear();
		folds.Clear();
	}

	private void RefreshComponentEditorCache()
	{
		var toRemove = new List<Component>();
		var toAdd = new List<Component>();

		foreach (var ed in editors)
			if(!componentEmbedder.toEmbed.Contains(ed.Key))
				toRemove.Add (ed.Key);

		foreach (var comp in componentEmbedder.toEmbed)
			if(comp != null && !editors.ContainsKey(comp))
				toAdd.Add(comp);

		foreach (var r in toRemove)
			RemoveEmbeddedComponent(r);

		foreach (var a in toAdd)
			AddEmbeddedComponent(a);
	}

	private void AddEmbeddedComponent(Component comp)
	{
		if (!editors.ContainsKey(comp))
			editors.Add(comp, CreateEditor(comp));

		if (!folds.ContainsKey(comp))
			folds.Add(comp, false);

        comp.hideFlags |= HideFlags.HideInInspector;
	}

	private void RemoveEmbeddedComponent(Component comp)
	{
		if(folds.ContainsKey(comp))
			folds.Remove(comp);
		
		if(editors.ContainsKey(comp))
		{
			DestroyImmediate(editors[comp]);
			editors.Remove(comp);
		}
		
		comp.hideFlags &= ~HideFlags.HideInInspector;
	}

	private void DrawComponentEditors()
	{
		foreach (var editor in editors)
		{
			using (var v = new EditorGUILayout.VerticalScope())
			{
				DrawComponentEditor(editor.Key, v);
			}
			EditorGUILayout.Space();
		}
	}

	private void DrawComponentEditor(Component component, EditorGUILayout.VerticalScope v)
	{
		GUI.Box(EditorGUI.IndentedRect(v.rect), "");				
		folds[component] = EditorGUILayout.InspectorTitlebar(folds[component], component, true);
		if(folds[component]) editors[component].OnInspectorGUI();
		EditorGUILayout.Space();
	}
}