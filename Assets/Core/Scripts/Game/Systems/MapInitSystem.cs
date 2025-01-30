using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LGrid;
using UnityEngine;

namespace Client.Game
{
    public class MapInitSystem : IEcsInitSystem
    {
        private EcsCustomInject<Map> _map;
        private EcsCustomInject<AllPools> _allPools;
        private const int Columns = 2; 
        private const int Rows = 1;

        public void Init(IEcsSystems systems)
        {
            var grid = Links.Instance.Grid;
            var cellSize = grid.cellSize;
            var cellGap = grid.cellGap;
            var effectiveCellSize = cellSize + cellGap;
            var gridOrigin = grid.transform.position;
        
            Gizmos.color = Color.white;
            for (var z = -Rows; z <= Rows; z++)
            {
                for (var x = -Columns; x < Columns; x++)
                {
                    var cellCenter = gridOrigin + new Vector3(
                        x * effectiveCellSize.x,
                        effectiveCellSize.y,
                        z * effectiveCellSize.z
                    );
                    if (_map.Value.IsCellExists(cellCenter, out _))
                    {
                        Debug.LogError("Cell already exists");
                    }
                    else
                    {
                        _allPools.Value.CarPlace.GetFromPool(cellCenter);
                        _map.Value.CreateCell(Vector3Int.RoundToInt(cellCenter));
                    }
                }
            }
        }
    }
}