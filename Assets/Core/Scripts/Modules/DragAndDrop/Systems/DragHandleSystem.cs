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
        private EcsCustomInject<Map> _map;
        private EcsFilterInject<Inc<EDragStart>> _eDragStart = "events";
        private EcsFilterInject<Inc<EDragEnd>> _eDragEnd = "events";

        private EcsFilterInject<Inc<CDragObject, CDragging>> _cDraggingFilter;
        
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
            ref var drag = ref _cDraggingFilter.Pools.Inc1.Get(packedDragEntity.FastUnpack());
            var transform = drag.DragAndDropMb.transform;
            var initialPoint = drag.LastDragInitialPoint;
            var cellToWorld = MapUtils.GetSnappedPosition(MapUtils.GetLimitedMousePosition());
            if (!_map.Value.IsCellExists(cellToWorld, out var cell) ||
                cellToWorld == initialPoint)
            {
                transform.position = initialPoint;
                return;
            }

            if (cell.TaxiBase == null)
            {
                if (_map.Value.IsCellExists(MapUtils.GetSnappedPosition(initialPoint), out var initialCell))
                {
                    initialCell.TaxiBase = null;
                }

                // cell.TaxiBase = _taxiBase;
                transform.position = cellToWorld;
                drag.LastDragInitialPoint = cellToWorld;
                return;
            }
        
            // if (cell.TaxiBase.Level == _taxiBase.Level &&
            //     !cell.TaxiBase.IsDriving)
            // {
            //     if (AllVehicles.Instance.CarsPool.Length <= _taxiBase.Level) Debug.Log("Max Level Reached!");
            //     Debug.Log($"Upgrade! from level {_taxiBase.Level} to {_taxiBase.Level+1}");
            //     if (_map.Value.IsCellExists(MapUtils.GetSnappedPosition(initialPoint), out var initialCell))
            //     {
            //         initialCell.TaxiBase = null;
            //     }
            //     MergeEffect(_taxiBase.transform, cell.TaxiBase.transform, cell);
            // }
            // else
            // {
            //     transform.position = MapUtils.GetSnappedPosition(initialPoint);
            // }    
        }
        
        // private void MergeEffect(Transform source, Transform target, Cell cell)
        // {
        //     const float xOffset = 2f;
        //     const float duration = 0.5f;
        //     source.position = target.position;
        //     var halfDuration = duration / 2f;
        //     Sequence.Create(2, CycleMode.Yoyo, Ease.OutSine)
        //         .Group(Tween.PositionX(source, source.position.x + xOffset, halfDuration))
        //         .Group(Tween.PositionX(target, target.position.x - xOffset, halfDuration))
        //         .OnComplete(() =>
        //         {
        //             source.gameObject.SetActive(false);
        //             target.gameObject.SetActive(false);
        //             AfterMergeEffect(cell);
        //         });
        // }
        //
        // private void AfterMergeEffect(Cell cell)
        // {
        //     var pool = AllVehicles.Instance.CarsPool[_taxiBase.Level];
        //     var createdObject = pool.GetFromPool(cell.Position);
        //     cell.TaxiBase = createdObject.GetComponent<TaxiBase>();
        //     var follower = createdObject.GetComponentInChildren<PathFollower>();
        //     Links.Instance.MergeEffect.GetFromPool(follower.transform.position);
        //     var mergeSound = Random.Range(1, 3) == 1 ? AllSfxSounds.Merge : AllSfxSounds.Merge2;
        //     SoundManager.Instance.PlayFX(mergeSound, cell.Position);
        // }
    }
}