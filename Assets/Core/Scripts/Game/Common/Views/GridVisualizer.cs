using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridMb : MonoBehaviour
{
    [HideInInspector] public Grid Grid;

    public int rows = 100;    // Количество ячеек по Z (в обе стороны)
    public int columns = 100; // Количество ячеек по X (в обе стороны)

    private void OnDrawGizmosSelected()
    {
        Grid = GetComponent<Grid>();
        var cellSize = Grid.cellSize;
        var cellGap = Grid.cellGap;
        var effectiveCellSize = cellSize + cellGap;
        var gridOrigin = Grid.transform.position;
        
        Gizmos.color = Color.white;
        for (var x = -columns; x <= columns; x++)
        {
            for (var z = -rows; z <= rows; z++)
            {
                var cellCenter = gridOrigin + new Vector3(
                    x * effectiveCellSize.x + cellSize.x / 2,
                    effectiveCellSize.y,
                    z * effectiveCellSize.z + cellSize.z / 2
                );
                DrawCellBoundary(cellCenter, effectiveCellSize);
            }
        }
    }

    private void DrawCellBoundary(Vector3 center, Vector3 size)
    {
        // Верхняя грань
        Gizmos.DrawLine(center + new Vector3(-size.x / 2, size.y / 2, -size.z / 2), center + new Vector3(size.x / 2, size.y / 2, -size.z / 2));
        Gizmos.DrawLine(center + new Vector3(-size.x / 2, size.y / 2, size.z / 2), center + new Vector3(size.x / 2, size.y / 2, size.z / 2));
        Gizmos.DrawLine(center + new Vector3(-size.x / 2, size.y / 2, -size.z / 2), center + new Vector3(-size.x / 2, size.y / 2, size.z / 2));
        Gizmos.DrawLine(center + new Vector3(size.x / 2, size.y / 2, -size.z / 2), center + new Vector3(size.x / 2, size.y / 2, size.z / 2));

        // Нижняя грань
        Gizmos.DrawLine(center + new Vector3(-size.x / 2, -size.y / 2, -size.z / 2), center + new Vector3(size.x / 2, -size.y / 2, -size.z / 2));
        Gizmos.DrawLine(center + new Vector3(-size.x / 2, -size.y / 2, size.z / 2), center + new Vector3(size.x / 2, -size.y / 2, size.z / 2));
        Gizmos.DrawLine(center + new Vector3(-size.x / 2, -size.y / 2, -size.z / 2), center + new Vector3(-size.x / 2, -size.y / 2, size.z / 2));
        Gizmos.DrawLine(center + new Vector3(size.x / 2, -size.y / 2, -size.z / 2), center + new Vector3(size.x / 2, -size.y / 2, size.z / 2));

        // Соединения между верхом и низом
        Gizmos.DrawLine(center + new Vector3(-size.x / 2, -size.y / 2, -size.z / 2), center + new Vector3(-size.x / 2, size.y / 2, -size.z / 2));
        Gizmos.DrawLine(center + new Vector3(size.x / 2, -size.y / 2, -size.z / 2), center + new Vector3(size.x / 2, size.y / 2, -size.z / 2));
        Gizmos.DrawLine(center + new Vector3(-size.x / 2, -size.y / 2, size.z / 2), center + new Vector3(-size.x / 2, size.y / 2, size.z / 2));
        Gizmos.DrawLine(center + new Vector3(size.x / 2, -size.y / 2, size.z / 2), center + new Vector3(size.x / 2, size.y / 2, size.z / 2));
    }
}
