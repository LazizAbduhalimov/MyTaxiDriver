using LGrid;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridDrawerWithGaps : MonoBehaviour
{
    private Grid grid;

    public int rows = 100;    // Количество ячеек по Z (в обе стороны)
    public int columns = 100; // Количество ячеек по X (в обе стороны)
    public int height = 100;  // Количество ячеек по Y (в обе стороны)

    private void OnDrawGizmos()
    {
        grid = GetComponent<Grid>();
        var cellSize = grid.cellSize;
        var cellGap = grid.cellGap;
        var effectiveCellSize = cellSize + cellGap;
        var gridOrigin = grid.transform.position;
        
        Gizmos.color = Color.gray;
        for (int x = -columns; x <= columns; x++)
        {
            for (int y = -height; y <= height; y++)
            {
                for (int z = -rows; z <= rows; z++)
                {
                    // Позиция центра ячейки
                    Vector3 cellCenter = gridOrigin + new Vector3(
                        x * effectiveCellSize.x,
                        y * effectiveCellSize.y,
                        z * effectiveCellSize.z
                    );
                    cellCenter.AddX(cellSize.x / 2).AddZ(cellSize.z / 2);
                    // Рисуем границы вокруг этой ячейки
                    DrawCellBoundary(cellCenter, effectiveCellSize);
                }
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
