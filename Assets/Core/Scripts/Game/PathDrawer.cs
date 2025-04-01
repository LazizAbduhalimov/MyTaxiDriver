using System;
using System.Collections.Generic;
using System.Linq;
using LGrid;
using UnityEngine;

namespace Core.Scripts.Game
{
    public class PathDrawer : MonoBehaviour
    {
        public Transform Unit;
        private PathFinder _pathFinder;

        private readonly Dictionary<Vector3Int, Tile> _tiles = new();
        private List<Cell> _path = new ();
        private List<Tile> _unSettableTiles = new();

        private List<Cell> _unitCells = new ();
        private bool isEnabled;

        private void Start()
        {
            var anchor = new Vector2Int(1, 1);
            var unitSize = new Vector2Int(2, 2);
            var tiles = FindObjectsOfType<Tile>();
            foreach (var tile in tiles)
            {
                _tiles.Add(Vector3Int.RoundToInt(tile.transform.position), tile);
            }
            var cellPosition = Vector3Int.RoundToInt(Unit.position);
            // Map.Instance.GetCell(cellPosition);
            // cell.IsOccupied = true;
            _pathFinder = new PathFinder(Map.Instance.Cells, unitSize, anchor);
            Unit.position = _pathFinder.GetUnitCenter(cellPosition, anchor);
            // Unit.localScale = new Vector3(unitSize.x, 1, unitSize.y);
        }
        
        public void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Path();
            }
            if (Input.GetKeyDown(KeyCode.Tab))
                isEnabled = !isEnabled;
            if (!isEnabled) return;
            Path();
        }

        private void Path()
        {
            var mousePosition = Vector3Int.RoundToInt(MapUtils.GetMouseWorldPosition());
            var start = Vector3Int.RoundToInt(Unit.position);
            PreviewPath(start, mousePosition);
            DoPath(start, mousePosition);
        }

        private void DoPath(Vector3Int start, Vector3Int finish)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Map.Instance.IsCellExists(finish, out var cell))
                {
                    if (cell.IsOccupied) return;
                    Unit.transform.position = _pathFinder.GetUnitCenter(finish, _pathFinder._anchor);
                }
                
                if (Map.Instance.IsCellExists(start, out cell))
                {
                    cell.IsOccupied = false;
                }
            }
        }

        private void PreviewPath(Vector3Int start, Vector3Int finish)
        {
            var path = _pathFinder.FindPath(start, finish);
            var placingCells = _pathFinder.GetPlacedCells(finish);
            foreach (var placingCell in placingCells)
            {
                if (Map.Instance.IsCellExists(placingCell, out var cell))
                {
                    var color = cell.IsOccupied ? Color.magenta : Color.cyan;
                    Debug.DrawRay(placingCell, Vector3.up, color);
                }
            }
            if (path.Count == 0)
            {
                if (_tiles.TryGetValue(finish, out var tile))
                {
                    _unSettableTiles.Add(tile);
                    tile.MeshRenderer.material.color = Color.red;
                }
            }
            else
            {
                foreach (var tile in _unSettableTiles)
                {
                    tile.MeshRenderer.material.color = Color.white;
                }
                _unSettableTiles.Clear();
                DrawPath(path);
            }
        }

        private void DrawPath(List<Cell> cells)
        {
            foreach (var cell in _path)
            {
                _tiles[cell.Position].MeshRenderer.material.color = Color.white;
            }
            
            foreach (var cell in cells)
            {
                _tiles[cell.Position].MeshRenderer.material.color = Color.green;
            }

            _path = cells;
        }
    }
}