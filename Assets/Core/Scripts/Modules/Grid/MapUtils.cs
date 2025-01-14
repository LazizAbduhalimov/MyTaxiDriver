using Leopotam.EcsLite;
using UnityEngine;

namespace LGrid
{
    public static class MapUtils
    {
        public static bool TryGetCellOccupier<T>(Vector3Int cell, EcsWorld world, out T standable) 
            where T: struct, ICellStandable
        {
            foreach (var entity in world.Filter<T>().End())
            {
                ref var data = ref world.GetPool<T>().Get(entity);
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