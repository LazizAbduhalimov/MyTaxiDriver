using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightPlaceMb : MonoBehaviour
{
    public MeshRenderer Mesh;
    public Color HighlightColor;
    private Color _defaultColor;

    private void Awake()
    {
        _defaultColor = Mesh.material.color;
    }

    public void Highlight()
    {
        Mesh.material.color = HighlightColor;
    }
    
    public void DisableHighlight()
    {
        Mesh.material.color = _defaultColor;
    }
}
