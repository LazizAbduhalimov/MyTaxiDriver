using System.Collections.Generic;
using System.Linq;
using LGrid;
using UnityEngine;

public class PathFinder
{
    private Dictionary<Vector3Int, Cell> _grid;
    private Vector2Int _unitSize;
    public Vector2Int _anchor;
    public Vector2Int _fixedAnchor;

    public Vector2Int[] AllAnchors = {
        new(1, 1),
        new(2, 1),
        new(1, 2),
        new(2, 2),
    };

    public PathFinder(Dictionary<Vector3Int, Cell> grid, Vector2Int unitSize, Vector2Int anchor)
    {
        _grid = grid;
        _unitSize = unitSize;
        _anchor = anchor;
        _fixedAnchor = anchor;
    }
    
    // public void SetAnchor()

    public List<Cell> FindPath(Vector3Int start, Vector3Int goal)
    {
        if (!CanPlaceUnit(start))
            return new List<Cell>();
        
        if (AllAnchors.All(a => !CanPlaceUnit(goal, a)))
        {
            return new List<Cell>();
        }

        var openSet = new SortedSet<(float, Vector3Int)>(Comparer<(float, Vector3Int)>.Create((a, b) =>
            a.Item1.CompareTo(b.Item1) != 0 ? a.Item1.CompareTo(b.Item1) : a.Item2.GetHashCode().CompareTo(b.Item2.GetHashCode()))) { (Heuristic(start, goal), start) };

        var cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        var gScore = new Dictionary<Vector3Int, float> { [start] = 0 };
        var fScore = new Dictionary<Vector3Int, float> { [start] = Heuristic(start, goal) };

        while (openSet.Count > 0)
        {
            var current = openSet.Min.Item2;
            openSet.Remove(openSet.Min);

            if (current == goal)
                return ReconstructPath(cameFrom, current);
            
            _anchor = GetAnchor(GetUnitCenter(current, _anchor), goal);

            foreach (var neighbor in GetNeighbors(current))
            {
                if (!CanPlaceUnit(neighbor))
                    continue;

                var tentativeGScore = gScore[current] + 1;

                if (!gScore.TryGetValue(neighbor, out var neighborGScore) || tentativeGScore < neighborGScore)
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + Heuristic(neighbor, goal);
                    openSet.Add((fScore[neighbor], neighbor));
                }
            }
        }
        
        return new List<Cell>(); // Если пути нет
    }

    private List<Cell> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        var path = new List<Cell> { _grid[current] };
        while (cameFrom.TryGetValue(current, out var prev))
        {
            current = prev;
            path.Add(_grid[current]);
        }
        path.Reverse();
        return path;
    }

    public static Vector2Int GetAnchor(Vector3 origin, Vector3 point)
    {
        var quadrant = GetQuadrant(origin, point);
        return quadrant switch
        {
            1 => new Vector2Int(2, 2),
            2 => new Vector2Int(1, 2),
            3 => new Vector2Int(1, 1),
            4 => new Vector2Int(2, 1),
            _ => new Vector2Int()
        };
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
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.z - b.z); // Манхэттенское расстояние (быстрее, чем `Vector3Int.Distance`)
    }

    private static readonly Vector3Int[] Directions =
    {
        Vector3Int.right, Vector3Int.left, Vector3Int.forward, Vector3Int.back
    };

    private List<Vector3Int> GetNeighbors(Vector3Int cell)
    {
        var neighbors = new List<Vector3Int>(8);

        foreach (var dir in Directions)
        {
            var neighbor = cell + dir;
            if (_grid.ContainsKey(neighbor))
                neighbors.Add(neighbor);
        }

        TryAddDiagonalNeighbor(neighbors, cell, Vector3Int.right, Vector3Int.forward);
        TryAddDiagonalNeighbor(neighbors, cell, Vector3Int.right, Vector3Int.back);
        TryAddDiagonalNeighbor(neighbors, cell, Vector3Int.left, Vector3Int.forward);
        TryAddDiagonalNeighbor(neighbors, cell, Vector3Int.left, Vector3Int.back);

        return neighbors;
    }

    private void TryAddDiagonalNeighbor(List<Vector3Int> neighbors, Vector3Int cell, Vector3Int dir1, Vector3Int dir2)
    {
        var diagonal = cell + dir1 + dir2;
        if (CanPlaceUnit(diagonal) && CanPlaceUnit(cell + dir1) && CanPlaceUnit(cell + dir2))
            neighbors.Add(diagonal);
    }

    private bool CanPlaceUnit(Vector3Int position, Vector2Int anchor)
    {
        for (var x = 0; x < _unitSize.x; x++)
        {
            for (var z = 0; z < _unitSize.y; z++)
            {
                var checkPos = new Vector3Int(position.x + x - (anchor.x - 1), position.y, position.z + z - (anchor.y - 1));
                if (!_grid.TryGetValue(checkPos, out var cell) || cell.IsOccupied)
                    return false;
            }
        }
        return true;
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
                var checkPos = new Vector3Int(position.x + x - (_anchor.x - 1), position.y, position.z + z - (_anchor.y - 1));
                placedCells[i++] = checkPos;
            }
        }
        return placedCells;
    }

    public Vector3 GetUnitCenter(Vector3Int cellPosition, Vector2Int anchor)
    {
        var centerX = cellPosition.x - (anchor.x - 1) + (_unitSize.x - 1) / 2f;
        var centerZ = cellPosition.z - (anchor.y - 1) + (_unitSize.y - 1) / 2f;
        return new Vector3(centerX, cellPosition.y, centerZ);
    }
}
