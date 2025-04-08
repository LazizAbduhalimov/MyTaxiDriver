using System;
using UnityEngine;

public class GridIndicator : MonoBehaviour
{
    private void Start()
    {
        ToGrid();
    }

    private void OnMouseExit()
    {
        ToGrid();
    }

    private void ToGrid()
    {
        var pos = Vector3Int.RoundToInt(GetMouseWorldPosition());
        var x = pos.x > transform.position.x ? -0.5f : 0.5f; 
        var z = pos.z > transform.position.z ? -0.5f : 0.5f; 
        transform.position = pos.ToVector3().AddX(x).AddZ(z);
    }
    
    public static Vector3 GetMouseWorldPosition()
    {
        var mouseScreenPosition = Input.mousePosition;
        var plane = new Plane(Vector3.up, Vector3.zero);
        var ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
        return plane.Raycast(ray, out var distance) ? ray.GetPoint(distance) : Vector3.zero;
    }
}