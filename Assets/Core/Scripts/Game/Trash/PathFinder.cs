using System.Collections.Generic;
using System.Linq;
using LGrid;
using UnityEngine;

public class PathFinder
{
    private Dictionary<Vector3Int, Cell> _grid;
    private Vector2Int _unitSize;
    private Vector2Int _placedAnchor;
    private Vector2Int _ghostAnchor;

    public PathFinder(Dictionary<Vector3Int, Cell> grid, Vector2Int unitSize, Vector2Int placedAnchor)
    {
        _grid = grid;
        _unitSize = unitSize;
        _placedAnchor = placedAnchor;
    }

    public List<Cell> FindPath(Vector3Int start, Vector3Int goal)
    {
        _ghostAnchor = _placedAnchor;
        if (!CanPlaceUnitAtGhostAnchor(start) || !CanPlaceUnitAtGhostAnchor(goal))
            return new List<Cell>();
        
        var anchor3d = new Vector3Int(_ghostAnchor.x, 0, _ghostAnchor.y);
        Debug.DrawRay(GetUnitCenter(start), Vector3.up, Color.magenta);
        var openSet = new HashSet<Vector3Int> { start };
        var cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        var gScore = new Dictionary<Vector3Int, float> { [start] = 0 };
        var fScore = new Dictionary<Vector3Int, float> { [start] = Heuristic(start, goal) };

        while (openSet.Count > 0)
        {
            var current = openSet.OrderBy(n => fScore.ContainsKey(n) ? fScore[n] : float.MaxValue).First();

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);

            foreach (var neighbor in GetNeighbors(current))
            {
                if (!CanPlaceUnitAtGhostAnchor(neighbor))
                    continue;

                var tentativeGScore = gScore[current] + Vector3Int.Distance(current, neighbor);

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + Heuristic(neighbor, goal);
                    openSet.Add(neighbor);
                }
            }
        }

        return new List<Cell>(); // Если пути нет
    }

    public static Vector2Int GetAnchor(Vector3 origin, Vector3 point)
    {
        var quadrant = GetQuadrant(origin, point);
        Debug.Log(quadrant);
        var anchor = quadrant switch
        {
            1 => new Vector2Int(2, 2),
            2 => new Vector2Int(1, 2),
            3 => new Vector2Int(1, 1),
            4 => new Vector2Int(2, 1),
            _ => new Vector2Int()
        };

        return anchor;
    }

    public static int GetQuadrant(Vector3 origin, Vector3 point)
    {
        var dx = point.x - origin.x;
        var dz = point.z - origin.z;
        Debug.Log($"dx = {dx}");
        Debug.Log($"dz = {dz}");
        if (dx >= 0 && dz >= 0) return 1;
        if (dx < 0 && dz >= 0) return 2;
        if (dx < 0 && dz < 0) return 3;
        return 4;
    }


    private float Heuristic(Vector3Int a, Vector3Int b)
    {
        return Vector3Int.Distance(a, b);
    }

    private List<Vector3Int> GetNeighbors(Vector3Int cell)
    {
        var neighbors = new List<Vector3Int>
        {
            cell + Vector3Int.right,
            cell + Vector3Int.left,
            cell + Vector3Int.forward,
            cell + Vector3Int.back
        };

        // Проверяем диагональные перемещения
        TryAddDiagonalNeighbor(neighbors, cell, Vector3Int.right, Vector3Int.forward);
        TryAddDiagonalNeighbor(neighbors, cell, Vector3Int.right, Vector3Int.back);
        TryAddDiagonalNeighbor(neighbors, cell, Vector3Int.left, Vector3Int.forward);
        TryAddDiagonalNeighbor(neighbors, cell, Vector3Int.left, Vector3Int.back);

        return neighbors;
    }

    private void TryAddDiagonalNeighbor(List<Vector3Int> neighbors, Vector3Int cell, Vector3Int dir1, Vector3Int dir2)
    {
        var diagonal = cell + dir1 + dir2;
        var check1 = cell + dir1;
        var check2 = cell + dir2;

        // Добавляем диагональ только если на всех трёх клетках юнит можно разместить
        if (CanPlaceUnitAtGhostAnchor(diagonal) && CanPlaceUnitAtGhostAnchor(check1) && CanPlaceUnitAtGhostAnchor(check2))
            neighbors.Add(diagonal);
    }
    
    private bool CanPlaceUnitAtGhostAnchor(Vector3Int position)
    {
        return CanPlaceUnit(position, _ghostAnchor);
    }
    
    private bool CanPlaceUnit(Vector3Int position, Vector2Int anchor)
    {
        var canPlace = true;
        for (var x = 0; x < _unitSize.x; x++)
        {
            for (var z = 0; z < _unitSize.y; z++)
            {
                var anchorPosX = anchor.x - 1;  
                var anchorPosY = anchor.y - 1;  
                var checkPos = new Vector3Int(position.x + x - anchorPosX, position.y, position.z + z - anchorPosY);
                
                if (!_grid.ContainsKey(checkPos) || _grid[checkPos].IsOccupied)
                    canPlace = false;
            }
        }
        return canPlace;
    }
    
    public Vector3Int[] GetPlacedCells(Vector3Int position, bool isGhostAnchor)
    {
        var placedCells = new Vector3Int[_unitSize.x * _unitSize.y];
        var i = 0;
        var anchor = isGhostAnchor ? _ghostAnchor : _placedAnchor;
        for (var x = 0; x < _unitSize.x; x++)
        {
            for (var z = 0; z < _unitSize.y; z++)
            {
                var anchorPosX = anchor.x - 1;  
                var anchorPosY = anchor.y - 1;  
                var checkPos = new Vector3Int(position.x + x - anchorPosX, position.y, position.z + z - anchorPosY);
                placedCells[i] = checkPos;
                i++;
            }
        }
        return placedCells;
    }

    private List<Cell> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        var path = new List<Cell> { _grid[current] };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(_grid[current]);
        }
        path.Reverse();
        return path;
    }
    
    public Vector3 GetUnitCenter(Vector3Int cellPosition)
    {
        // Предполагается, что _unitSize.x - количество клеток по оси X (ширина),
        // а _unitSize.y - количество клеток по оси Z (глубина) 
        var centerX = cellPosition.x - (_placedAnchor.x - 1) + (_unitSize.x -1) / 2f;
        var centerZ = cellPosition.z - (_placedAnchor.y - 1) + (_unitSize.y -1) / 2f;
        // Оставляем высоту без изменений (anchor.y)
        return new Vector3(centerX, cellPosition.y, centerZ);
    }
}
