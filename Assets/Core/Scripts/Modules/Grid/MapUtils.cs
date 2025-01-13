using Leopotam.EcsLite;
using UnityEngine;

namespace LGrid
{
    public static class MapUtils
    {
        public static EcsWorld World;
        
        public static bool TryGetCellOccupier<T>(Vector3Int cell, out T standable) 
            where T: struct, ICellStandable
        {
            foreach (var entity in World.Filter<T>().End())
            {
                ref var data = ref World.GetPool<T>().Get(entity);
                if (data.Coords == cell)
                {
                    standable = data;
                    return true;
                }
            }

            standable = default;
            return false;
        }
    }
}