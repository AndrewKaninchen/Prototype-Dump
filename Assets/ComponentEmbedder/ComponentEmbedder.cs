using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Note: this is not an editor script.
[ExecuteInEditMode]
public class ComponentEmbedder : MonoBehaviour
{
	#if UNITY_EDITOR
	public List<Component> toEmbed = new List<Component>();	

    private void OnDestroy()
    {
        foreach (var comp in toEmbed)
        {
            comp.hideFlags &= ~HideFlags.HideInInspector;
        }
    }

#endif
}