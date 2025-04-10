using Client.Game.Test;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LGrid;
using UnityEngine;

namespace Client.Game
{
    public class HighlightPlaceSystem : IEcsRunSystem
    {
        private EcsCustomInject<Map> _map;
        private EcsFilterInject<Inc<EDragStart>> _eDragStart = "events";
        private EcsFilterInject<Inc<EDragEnd>> _eDragEnd = "events";
        private EcsFilterInject<Inc<CDragObject, CDragging>> _cDraggingFilter;
        private EcsFilterInject<Inc<CHighlightPlace>> _cHighlightFilter;

        private HighlightPlaceMb _initial;
        private HighlightPlaceMb _lastPlace;

        public void Run(IEcsSystems systems)
        {
            foreach (var _ in _eDragStart.Value) HighlightInitial();
            foreach (var _ in _cDraggingFilter.Value) HighlightSelected();
            foreach (var _ in _eDragEnd.Value) ClearHighlight();
        }

        private void HighlightInitial()
        {
            foreach (var dragEntity in _cDraggingFilter.Value)
            {
                var dragData = _cDraggingFilter.Pools.Inc1.Get(dragEntity);
                var position = dragData.LastDragInitialPoint;

                foreach (var highlightEntity in _cHighlightFilter.Value)
                {
                    var placeData = _cHighlightFilter.Pools.Inc1.Get(highlightEntity);
                    var place = placeData.HighlightPlaceMb;

                    if (place.transform.position == position)
                    {
                        _initial = place;
                        place.Highlight();
                        return;
                    }
                }
            }
        }

        private void HighlightSelected()
        {
            var selectedCell = MapUtils.GetSnappedMousePosition();

            foreach (var highlightEntity in _cHighlightFilter.Value)
            {
                var placeData = _cHighlightFilter.Pools.Inc1.Get(highlightEntity);
                var place = placeData.HighlightPlaceMb;

                if (place == _lastPlace || place == _initial) continue;
                if (placeData.CellPosition != selectedCell) continue;

                place.Highlight();
                _lastPlace?.DisableHighlight();
                _lastPlace = place;
                return;
            }
        }

        private void ClearHighlight()
        {
            _initial?.DisableHighlight();
            _lastPlace?.DisableHighlight();
            _initial = null;
            _lastPlace = null;
        }
    }
}
