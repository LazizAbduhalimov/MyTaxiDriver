using System;
using System.Collections.Generic;
using System.Linq;
using LGrid;
using UnityEngine;

public class DebugFrame
{
    public Vector3 UnitCenter;
    public Vector3Int Current;
    public List<Vector3Int> Neighbours = new ();
    public List<Vector2Int> NeighboursAnchor = new();
}

public class PathFinder
{
    public Vector2Int Anchor;
    public Vector2Int StartAnchor;
    public static readonly List<DebugFrame> Frames = new ();
    
    private static readonly Vector3Int[] Directions = {
        Vector3Int.right, 
        Vector3Int.left, 
        Vector3Int.forward, 
        Vector3Int.back
    };
    
    private static readonly List<Vector3Int> Corners = new()
    {
        new Vector3Int(-1, 0,  1),
        new Vector3Int( 1, 0,  1),
        new Vector3Int(-1, 0, -1),
        new Vector3Int( 1, 0, -1)
    };
    
    private static readonly Vector2Int[] All2By2Anchors = {
        new(1, 1),
        new(2, 1),
        new(1, 2),
        new(2, 2),
    };
    
    private Dictionary<Vector3Int, Cell> _grid;
    private Vector2Int _unitSize;

    public PathFinder(Dictionary<Vector3Int, Cell> grid, Vector2Int unitSize, Vector2Int anchor)
    {
        _grid = grid;
        _unitSize = unitSize;
        Anchor = anchor;
        StartAnchor = anchor;
    }
    
    public List<Cell> FindPath(Vector3Int start, Vector3Int goal)
    {
        Anchor = StartAnchor;
        if (!CanPlaceUnit(start, Anchor))
            return new List<Cell>();
        
        if (All2By2Anchors.All(a => !CanPlaceUnit(goal, a)))
            return new List<Cell>();

        // такой тип нужен для оптимизации чтобы не сортировывать каждый раз
        var openSet = new SortedSet<(float, Vector3Int, Vector2Int)>(Comparer<(float, Vector3Int, Vector2Int)>.Create((a, b) =>
            a.Item1.CompareTo(b.Item1) != 0 ? a.Item1.CompareTo(b.Item1) : a.Item2.GetHashCode().CompareTo(b.Item2.GetHashCode()))) { (Heuristic(start, goal), start, Anchor) };

        var cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        var cameFromCenter = new Dictionary<Vector3, Vector3>();
        var gScore = new Dictionary<Vector3Int, float> { [start] = 0 };
        var fScore = new Dictionary<Vector3Int, float> { [start] = Heuristic(start, goal) };
        Frames.Clear();
        while (openSet.Count > 0)
        {
            var debugFrame = new DebugFrame();
            
            var current = openSet.Min.Item2;
            var unitCenter = GetUnitCenter(current, openSet.Min.Item3);
            openSet.Remove(openSet.Min);
            
            debugFrame.Current = current;
            debugFrame.UnitCenter = unitCenter;
            
            if (current == goal)
            {
                return ReconstructPath(cameFrom, cameFromCenter, current, unitCenter);
            }

            var anchor = Anchor;
            foreach (var neighbor in GetNeighbors(current))
            {
                Anchor = GetAnchor(unitCenter, neighbor);
                debugFrame.Neighbours.Add(neighbor);
                debugFrame.NeighboursAnchor.Add(Anchor);
                if (!CanPlaceUnit(neighbor, Anchor))
                    continue;

                var tentativeGScore = gScore[current] + 1;

                if (!gScore.TryGetValue(neighbor, out var neighborGScore) || tentativeGScore < neighborGScore)
                {
                    cameFromCenter[neighbor] = unitCenter;
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + Heuristic(neighbor, goal);
                    openSet.Add((fScore[neighbor], neighbor, Anchor));
                }
            }
            Frames.Add(debugFrame);
            Anchor = anchor;
        }
        
        return new List<Cell>();
    }

    private List<Cell> ReconstructPath(
        Dictionary<Vector3Int, Vector3Int> cameFrom, 
        Dictionary<Vector3, Vector3> cameFromCenter, 
        Vector3Int current, 
        Vector3 unitCenter)
    {
        var pathCenter = new List<Vector3> { unitCenter };
        var path = new List<Cell> { _grid[current] };
        
        while (cameFrom.TryGetValue(current, out var prev))
        {
            pathCenter.Add(cameFromCenter[current]);
            current = prev;
            path.Add(_grid[current]);
        }
        path.Reverse();
        pathCenter.Reverse();
        //TODO: Remove after path lerping
        foreach (var pos in pathCenter) 
            Debug.DrawRay(pos, Vector3.up, Color.yellow, 1f);
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
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.z - b.z);
    }

    private List<Vector3Int> GetNeighbors(Vector3Int cell)
    {
        var neighbors = new List<Vector3Int>(8);
        neighbors.AddRange(Directions.Select(dir => cell + dir).Where(neighbor => _grid.ContainsKey(neighbor)));

        TryAddDiagonalNeighbor(neighbors, cell, Vector3Int.right, Vector3Int.forward);
        TryAddDiagonalNeighbor(neighbors, cell, Vector3Int.right, Vector3Int.back);
        TryAddDiagonalNeighbor(neighbors, cell, Vector3Int.left, Vector3Int.forward);
        TryAddDiagonalNeighbor(neighbors, cell, Vector3Int.left, Vector3Int.back);

        return neighbors;
    }
    
    private Vector3Int[] GetNeighborCorners(Vector3Int corner)
    {
        if (!Corners.Contains(corner))
            return Array.Empty<Vector3Int>();

        // Возвращаем только те углы, что НЕ равны текущему и НЕ противоположны ему
        return Corners
            .Where(c => c != corner && (c.x == corner.x || c.z == corner.z)).ToArray();
    }

    private Cell[] GetAdjacentCornersCells(Vector3Int origin, Vector3Int destination)
    {
        var corner = destination - origin;
        var adjacentCorners = GetNeighborCorners(corner);
        var cells = new List<Cell>();
        foreach (var adjacentCorner in adjacentCorners)
        {
            if (Map.Instance.IsCellExists(origin + adjacentCorner, out var cell))
            {
                cells.Add(cell);
            }
        }
        return cells.ToArray();
    }

    private void TryAddDiagonalNeighbor(List<Vector3Int> neighbors, Vector3Int cell, Vector3Int dir1, Vector3Int dir2)
    {
        var diagonal = cell + dir1 + dir2;
        var direction1 = cell + dir1;
        var direction2 = cell + dir2;

        var anchorDiagonal = GetAnchor(cell, diagonal);
        var anchor1 = GetAnchor(cell, direction1);
        var anchor2 = GetAnchor(cell, direction2);
        
        var unitCenter = GetUnitCenter(cell, Anchor);
        // если двигаемся диагонально проверяем нет ли премятствий у смежных углов
        // у нас 2 на 2 жирный объект и проверяется не только соседи якоря но и соседи соседей нашего якоря ;)
        // сложно и муторно крч, но так меньше лагов
        if ((diagonal - unitCenter).sqrMagnitude > 4)
            if (GetAdjacentCornersCells(cell, diagonal).Any(c => c.IsOccupied))
                return;

        if (CanPlaceUnit(diagonal, anchorDiagonal) && CanPlaceUnit(direction1, anchor1) && CanPlaceUnit(direction2, anchor2))
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

    public Vector3Int[] GetPlacedCells(Vector3Int position)
    {
        var placedCells = new Vector3Int[_unitSize.x * _unitSize.y];
        var i = 0;
        for (var x = 0; x < _unitSize.x; x++)
        {
            for (var z = 0; z < _unitSize.y; z++)
            {
                var checkPos = new Vector3Int(position.x + x - (Anchor.x - 1), position.y, position.z + z - (Anchor.y - 1));
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
