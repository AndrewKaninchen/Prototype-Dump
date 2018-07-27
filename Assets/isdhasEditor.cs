using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(isdhas))]
public class isdhasEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var t = target as isdhas;
        base.OnInspectorGUI();

        t.text = EditorGUILayout.TextArea(t.text);// options: new []{GUILayout.ExpandHeight(true)});
    }
}
