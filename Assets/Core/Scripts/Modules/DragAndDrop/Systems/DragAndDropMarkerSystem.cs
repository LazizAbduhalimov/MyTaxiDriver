using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client.Game.Test
{
    public class DragAndDropMarkerSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<EDragStart>> _eDragStart = "events";
        private EcsFilterInject<Inc<EDragEnd>> _eDragEnd = "events";

        private EcsPoolInject<CDragObject> _cDragObject;
        private EcsPoolInject<CDragging> _cDragging;
            
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eDragStart.Value) MarkAsDragging(entity);
            foreach (var entity in _eDragEnd.Value) UnmarkDragging(entity);
        }

        private void MarkAsDragging(int entity)
        {
            ref var dragData = ref _eDragStart.Pools.Inc1.Get(entity);
            var unpacked = dragData.PackedEntity.FastUnpack();
            ref var dragObject = ref _cDragObject.Value.Get(unpacked);
            dragObject.LastDragInitialPoint = dragObject.DragAndDropMb.transform.position;
            _cDragging.Value.TryAdd(unpacked, out _);
            Debug.Log($"Start dragging {dragObject.DragAndDropMb.name}");
        }
        
        private void UnmarkDragging(int entity)
        {
            ref var dragData = ref _eDragEnd.Pools.Inc1.Get(entity);
            var unpacked = dragData.PackedEntity.FastUnpack();
            ref var dragObject = ref _cDragObject.Value.Get(unpacked);
            _cDragging.Value.Del(unpacked);
            Debug.Log($"End dragging {dragObject.DragAndDropMb.name}");
        }
    }
}