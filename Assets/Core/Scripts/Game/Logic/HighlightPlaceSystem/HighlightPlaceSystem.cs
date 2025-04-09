using Client.Game.Test;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LGrid;
using UnityEngine;

namespace Client.Game
{
    public class HighlightPlaceSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<EDragStart>> _eDragStart = "events";
        private EcsFilterInject<Inc<EDragEnd>> _eDragEnd = "events";
        private EcsFilterInject<Inc<CDragObject, CDragging>> _cDraggingFilter;
        private EcsFilterInject<Inc<CHighlightPlace>> _cHighlightFilter;

        private HighlightPlaceMb _initial;
        private HighlightPlaceMb _lastPlace;
        private Vector3 _lastCell;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eDragStart.Value) HighlightInitial(entity);
            foreach (var entity in _cDraggingFilter.Value) HighlightSelected(entity);
            foreach (var entity in _eDragEnd.Value) DisableHighlight(entity);
        }

        private void HighlightInitial(int entity)
        {
            foreach (var dragEntity in _cDraggingFilter.Value)
            {
                ref var dragData = ref _cDraggingFilter.Pools.Inc1.Get(dragEntity);
                var position = dragData.LastDragInitialPoint;
                foreach (var highlightEntity in _cHighlightFilter.Value)
                {
                    var place = _cHighlightFilter.Pools.Inc1.Get(highlightEntity).HighlightPlaceMb;
                    if (place.transform.position == position)
                        place.Highlight();
                }
            }
        }

        private void HighlightSelected(int entity)
        {
            foreach (var dragEntity in _cDraggingFilter.Value)
            {
                var selectedCell = MapUtils.GetSnappedMousePosition();
                if (selectedCell != _lastCell)
                {
                    if (Map.Instance.IsCellExists(selectedCell, out var cell))
                    {
                        foreach (var highlightEntity in _cHighlightFilter.Value)
                        {
                            var place = _cHighlightFilter.Pools.Inc1.Get(highlightEntity).HighlightPlaceMb;
                            if (place.transform.position == selectedCell)
                            {
                                place.Highlight();
                                _lastCell = selectedCell;
                                _lastPlace = place;
                            }
                        }
                    }
                }
            }
        }
        
        private void DisableHighlight(int entity)
        { 
            _initial.DisableHighlight();
            _lastPlace.DisableHighlight();
        }
    }
}