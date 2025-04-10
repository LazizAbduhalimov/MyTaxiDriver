using UnityEngine;

namespace Client.Game
{
    public struct CHighlightPlace
    {
        public Vector3 CellPosition;
        public HighlightPlaceMb HighlightPlaceMb;

        public void Invoke(Vector3 cellPosition, HighlightPlaceMb highlightPlaceMb)
        {
            CellPosition = cellPosition;
            HighlightPlaceMb = highlightPlaceMb;
        }
    }
}