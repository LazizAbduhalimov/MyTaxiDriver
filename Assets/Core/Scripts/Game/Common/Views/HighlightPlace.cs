using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightPlace : MonoBehaviour
{
    public MeshRenderer Mesh;
    public Color HighlightColor;
    private Color _defaultColor;

    private void Awake()
    {
        _defaultColor = Mesh.material.color;
    }

    private void OnMouseEnter()
    {
        if (Input.GetMouseButton(0))
        {
            Mesh.material.color = HighlightColor;
            // Debug.Log("Changing");
        }
    }

    private void OnMouseExit()
    {
        Mesh.material.color = _defaultColor;
    }
}
