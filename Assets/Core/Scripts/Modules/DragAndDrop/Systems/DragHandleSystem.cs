using Client.Game.Test;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    public class DragHandleSystem : IEcsRunSystem
    {
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
            transform.position = Utilities.GetMouseWorldPosition();
        }
        
        private void Drop(int entity)
        {
            
        }
    }
}