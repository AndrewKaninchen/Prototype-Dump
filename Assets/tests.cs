using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class tests : MonoBehaviour {

    public GUIStyle helpBox = EditorStyles.helpBox;
    public GUIStyle foldout = EditorStyles.foldout;

    private void Update()
    {
        helpBox = EditorStyles.helpBox;
        foldout = EditorStyles.foldout;
    }
}
