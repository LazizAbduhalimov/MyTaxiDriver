using System.Collections.Generic;
using System.Linq;
using LGrid;
using UnityEngine;

public class PathFinder
{
    private Dictionary<Vector3Int, Cell> _grid;
    private Vector2Int _unitSize;
    public Vector2Int _anchor;
    private Vector2Int _fixedAnchor;

    private List<Vector2Int> AllAnchors = new List<Vector2Int>()
    {
        new Vector2Int(1, 1),
        new Vector2Int(2, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, 1),
    }; 

    public PathFinder(Dictionary<Vector3Int, Cell> grid, Vector2Int unitSize, Vector2Int anchor)
    {
        _grid = grid;
        _unitSize = unitSize;
        _anchor = anchor;
        _fixedAnchor = anchor;
    }

    public List<Cell> FindPath(Vector3Int start, Vector3Int goal)
    {
        // _anchor = GetAnchor(GetUnitCenter(start, _anchor), goal);
        //TODO: блокируется один из способов расстановки 
        if (!CanPlaceUnit(start) || !CanPlaceUnit(goal))
            return new List<Cell>();

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

            _anchor = GetAnchor(GetUnitCenter(current, _anchor), goal);
            Debug.Log(_anchor);
            foreach (var neighbor in GetNeighbors(current))
            {
                if (!CanPlaceUnit(neighbor))
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
    
    public static Vector2Int GetAnchor(Vector3 origin, Vector3 point)
    {
        var quadrant = GetQuadrant(origin, point);
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
        if (CanPlaceUnit(diagonal) && CanPlaceUnit(check1) && CanPlaceUnit(check2))
            neighbors.Add(diagonal);
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

    private bool CanPlaceUnit(Vector3Int position)
    {
        return CanPlaceUnit(position, _anchor);
    }
    
    public Vector3Int[] GetPlacedCells(Vector3Int position)
    {
        var placedCells = new Vector3Int[_unitSize.x * _unitSize.y];
        var i = 0;
        for (var x = 0; x < _unitSize.x; x++)
        {
            for (var z = 0; z < _unitSize.y; z++)
            {
                var anchorPosX = _anchor.x - 1;  
                var anchorPosY = _anchor.y - 1;  
                var checkPos = new Vector3Int(position.x + x - anchorPosX, position.y, position.z + z - anchorPosY);
                placedCells[i] = checkPos;
                i++;
            }
        }
        return placedCells;
    }
    
    public Vector3 GetUnitCenter(Vector3Int cellPosition, Vector2Int anchor)
    {
        var centerX = cellPosition.x - (anchor.x - 1) + (_unitSize.x -1) / 2f;
        var centerZ = cellPosition.z - (anchor.y - 1) + (_unitSize.y -1) / 2f;
        return new Vector3(centerX, cellPosition.y, centerZ);
    }
}
