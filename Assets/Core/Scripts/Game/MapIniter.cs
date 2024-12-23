using System;
using LGrid;
using PoolSystem.Alternative;
using UnityEngine;

namespace Client.Game
{
    public class MapIniter : MonoBehaviour
    {
        [SerializeField] private PoolContainer _highlightPlaces;
        private int columns = 2; 
        private int rows = 1;
        
        public void Init()
        {
            var grid = Links.Instance.Grid;
            var cellSize = grid.cellSize;
            var cellGap = grid.cellGap;
            var effectiveCellSize = cellSize + cellGap;
            var gridOrigin = grid.transform.position;
        
            Gizmos.color = Color.white;
            for (var x = -columns; x < columns; x++)
            {
                for (var z = -rows; z <= rows; z++)
                {
                    var cellCenter = gridOrigin + new Vector3(
                        x * effectiveCellSize.x,
                        effectiveCellSize.y,
                        z * effectiveCellSize.z
                    );
                    if (Map.Instance.IsCellExists(cellCenter, out _))
                    {
                        Debug.LogError("Cell already exists");
                    }
                    else
                    {
                        var cell = _highlightPlaces.GetFromPool(cellCenter);
                        // cell.transform.localScale = grid.cellSize;
                        Map.Instance.CreateCell(Vector3Int.RoundToInt(cellCenter));
                    }
                }
            }
        }
    }
}