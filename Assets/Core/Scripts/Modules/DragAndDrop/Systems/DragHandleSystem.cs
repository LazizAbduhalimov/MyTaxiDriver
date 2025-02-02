using Client.Game;
using Client.Game.Test;
using Core.Scripts.Game;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LGrid;
using PrimeTween;
using UnityEngine;

namespace Client
{
    public class DragHandleSystem : IEcsRunSystem
    {
        private EcsWorldInject _world;
        private EcsCustomInject<Map> _map;
        private EcsCustomInject<AllPools> _allPools;
        private EcsFilterInject<Inc<EDragStart>> _eDragStart = "events";
        private EcsFilterInject<Inc<EDragEnd>> _eDragEnd = "events";

        private EcsFilterInject<Inc<CDragObject, CDragging>> _cDraggingFilter;

        private EcsPoolInject<CActive> _cActive;
        private EcsPoolInject<CTaxi> _cTaxi;
        private EcsPoolInject<EMerged> _eMerged = "events";

        private const float XOffset = 2f;
        private const float Duration = 0.5f;
        private const float HalfDuration = Duration / 2f;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _cDraggingFilter.Value) Drag(entity);
            foreach (var entity in _eDragEnd.Value) Drop(entity);
        }

        private void Drag(int entity)
        {
            ref var drag = ref _cDraggingFilter.Pools.Inc1.Get(entity);
            var transform = drag.DragAndDropMb.transform;
            transform.position = MapUtils.GetMouseWorldPosition();
        }
        
        private void Drop(int entity)
        {
            var packedDragEntity = _eDragEnd.Pools.Inc1.Get(entity).PackedEntity;
            var dragEntity = packedDragEntity.FastUnpack();
            ref var drag = ref _cDraggingFilter.Pools.Inc1.Get(dragEntity);
            ref var cTaxi = ref _cTaxi.Value.Get(dragEntity);
            var transform = drag.DragAndDropMb.transform;
            var initialCell = _map.Value.GetCell(drag.LastDragInitialPoint);
            var cellToWorld = MapUtils.GetSnappedPosition(MapUtils.GetLimitedMousePosition());
            if (!_map.Value.IsCellExists(cellToWorld, out var cell) || cellToWorld == initialCell.Position)
            {
                transform.position = initialCell.Position;
                return;
            }

            if (!MapUtils.TryGetCellOccupier<CTaxi>(cell.Position, _world.Value, out var droppedCellOccupier))
            {
                cell.IsOccupied = true;
                initialCell.IsOccupied = false;
                transform.position = cellToWorld;
                drag.LastDragInitialPoint = cellToWorld;
                return;
            }
                
            var draggedTaxi = cTaxi.TaxiMb;
            var mergingTaxi = droppedCellOccupier.TaxiMb;
            if (_allPools.Value.CarsPool.Length <= draggedTaxi.Level)
            {
                Debug.Log("Max Level Reached!");
            }
            else if (mergingTaxi.Level == draggedTaxi.Level)
            {
                initialCell.IsOccupied = false;
                MergeAnimation(draggedTaxi, mergingTaxi);
                return;
            }
            
            transform.position = initialCell.Position;
        }
        
        private void MergeAnimation(TaxiMb dragged, TaxiMb merging)
        {
            Debug.Log($"Upgrade! from level {dragged.Level} to {dragged.Level+1}");
            var dragTransform = dragged.transform;
            var mergeTransform = merging.transform;
            dragTransform.position = mergeTransform.position;
            dragged.GetDragAndDropMb().SetEnabled(false);
            merging.GetDragAndDropMb().SetEnabled(false);
            Sequence.Create(2, CycleMode.Yoyo, Ease.OutSine)
                .Group(Tween.PositionX(dragTransform, dragTransform.position.x + XOffset, HalfDuration))
                .Group(Tween.PositionX(mergeTransform, mergeTransform.position.x - XOffset, HalfDuration))
                .OnComplete(() => PlayMergeEffect(dragged, merging));
        }
        
        private void PlayMergeEffect(TaxiMb dragged, TaxiMb merging)
        {
            _cActive.Value.Del(dragged.PackedEntity.FastUnpack());
            _cActive.Value.Del(merging.PackedEntity.FastUnpack());
            dragged.GetDragAndDropMb().SetEnabled(true);
            merging.GetDragAndDropMb().SetEnabled(true);
            dragged.gameObject.SetActive(false);
            merging.gameObject.SetActive(false);
            
            var pool = _allPools.Value.CarsPool[dragged.Level];
            var taxiMb = pool.GetFromPool(merging.transform.position);
            taxiMb.Drive();
            _cActive.Value.Add(taxiMb.PackedEntity.FastUnpack());
            
            _eMerged.NewEntity(out _).Invoke(dragged, merging);
        }
        
        // private void FillCell(Vector3Int cellPosition)
        // {
        //     if (!Map.Instance.IsCellExists(cellPosition, out var cell))
        //     {
        //         cell = Map.Instance.CreateCell(cellPosition);
        //     }
        //     else
        //     {
        //         if (cell.TaxiMb != null)
        //         {
        //             var emptyCell = Map.Instance.Cells.Values.FirstOrDefault(c => c.TaxiMb == null);
        //             if (emptyCell == null)
        //             {
        //                 Debug.LogError("Too many objects");
        //                 gameObject.SetActive(false);
        //                 return;
        //             }
        //
        //             cell = emptyCell;
        //         }
        //     }
        //     cell.TaxiMb = _taxiMb;
        //     transform.position = cell.Position;
        // }
    }
}