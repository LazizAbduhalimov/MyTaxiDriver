using Client.Game;
using Client.Game.Test;
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

        private const float XOffset = 2f;
        private const float Duration = 0.5f;
        private const float HalfDuration = Duration / 2f;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _cDraggingFilter.Value) Drag(entity);
            foreach (var entity in _eDragEnd.Value) Drop(entity);

            #region Debug
            if (Input.GetMouseButtonDown(1))
            {
                var cellPos = Vector3Int.RoundToInt(MapUtils.GetSnappedMousePosition());
                if (MapUtils.TryGetCellOccupier<CTaxi>(cellPos, _world.Value, out var standable))
                {
                    Debug.Log($"Have standable {standable.TaxiMb.name}");
                }
                else
                {
                    Debug.Log("Cell is empty");
                }
                if (_map.Value.IsCellExists(cellPos, out var cell))
                {
                    Debug.Log($"Cell is occupied {cell.IsOccupied}");
                }
            }
            #endregion
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
            ref var drag = ref _cDraggingFilter.Pools.Inc1.Get(packedDragEntity.FastUnpack());
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
                
            var draggedTaxi = drag.DragAndDropMb.GetComponent<TaxiMb>();
            var mergingTaxi = droppedCellOccupier.TaxiMb;
            if (_allPools.Value.CarsPool.Length <= draggedTaxi.Level)
            {
                Debug.Log("Max Level Reached!");
            }
            else if (mergingTaxi.Level == draggedTaxi.Level)
            {
                initialCell.IsOccupied = false;
                MergeAnimation(draggedTaxi, mergingTaxi, draggedTaxi.Level);
                return;
            }
            
            transform.position = initialCell.Position;
        }
        
        private void MergeAnimation(TaxiMb dragged, TaxiMb merging, int carLevel)
        {
            Debug.Log($"Upgrade! from level {dragged.Level} to {dragged.Level+1}");
            var dragTransfrom = dragged.transform;
            var mergeTransform = merging.transform;
            dragTransfrom.position = mergeTransform.position;
            Sequence.Create(2, CycleMode.Yoyo, Ease.OutSine)
                .Group(Tween.PositionX(dragTransfrom, dragTransfrom.position.x + XOffset, HalfDuration))
                .Group(Tween.PositionX(mergeTransform, mergeTransform.position.x - XOffset, HalfDuration))
                .OnComplete(() =>
                {
                    _cActive.Value.Del(dragged.PackedEntity.FastUnpack());
                    _cActive.Value.Del(merging.PackedEntity.FastUnpack());
                    dragged.gameObject.SetActive(false);
                    merging.gameObject.SetActive(false);
                    PlayMergeEffect(dragged, merging, carLevel);
                });
        }
        
        private void PlayMergeEffect(TaxiMb dragged, TaxiMb merging, int carLevel)
        {
            var pool = _allPools.Value.CarsPool[carLevel];
            var taxiMb = pool.GetFromPool(merging.transform.position);
            taxiMb.Drive();
            _cActive.Value.Add(taxiMb.PackedEntity.FastUnpack());
            _allPools.Value.MergeEffect.GetFromPool(dragged.Follower.transform.position);
            _allPools.Value.MergeEffect.GetFromPool(merging.Follower.transform.position);
            _allPools.Value.MergeEffect.GetFromPool(merging.TransparentGfx.transform.position);
            var mergeSound = Random.Range(1, 3) == 1 ? AllSfxSounds.Merge : AllSfxSounds.Merge2;
            SoundManager.Instance.PlayFX(mergeSound, merging.transform.position);
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