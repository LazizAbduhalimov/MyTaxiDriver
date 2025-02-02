using Client.Game;
using UnityEngine;

namespace LGrid
{
    public class Cell
    {
        public Vector3Int Position;
        public TaxiMb TaxiMb;
        public bool IsOccupied;

        public Cell(Vector3Int position)
        {
            Position = position;
        }
    }
}